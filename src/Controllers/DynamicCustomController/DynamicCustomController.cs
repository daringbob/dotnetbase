
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;

namespace src.Controllers.DynamicCustomController
{
    [Authorize]
    public class DynamicCustomController<TEntity> : ControllerBase where TEntity : class, IAuditableEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public DynamicCustomController(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        [HttpPost("BulkInsert")]
        public async Task<IActionResult> BulkInsert([FromBody] List<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return BadRequest("The list is empty or null.");
            }

            // Sử dụng transaction để đảm bảo tính toàn vẹn của dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var startTime = DateTime.UtcNow; // Lưu thời gian bắt đầu
                try
                {
                    await _context.BulkInsertAsync(entities, new BulkConfig
                    {
                        UseTempDB = true,
                        BatchSize = entities.Count,
                        SetOutputIdentity = true
                    });

                    await transaction.CommitAsync();

                    var endTime = DateTime.UtcNow; // Lưu thời gian kết thúc
                    var elapsedTime = (endTime - startTime).TotalMilliseconds; // Tính thời gian thực thi tính bằng mili giây

                    // Ghi lại thời gian thực thi
                    Console.WriteLine($"Bulk insert completed in {elapsedTime} ms."); // Ghi log thời gian

                    Console.Write(entities);
                    return Ok(entities);
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }



        [HttpPost("BulkPatch")]
        public async Task<IActionResult> BulkPatch([FromBody] List<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return BadRequest("The list of entities cannot be empty.");
            }

            // Lists to store entities to be inserted and updated
            var entitiesToInsert = new List<TEntity>();
            var entitiesToUpdate = new List<TEntity>();

            // Use reflection to check for 'Id' property and split the entities
            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null)
            {
                return BadRequest("Entity does not have an Id property.");
            }

            foreach (var entity in entities)
            {
                var idValue = idProperty.GetValue(entity);

                // Check if the entity should be inserted or updated
                if (idValue == null || (int)idValue == 0)
                {
                    // If Id is null or 0, it's a new entity (insert)
                    entitiesToInsert.Add(entity);
                }
                else
                {
                    // If Id exists, it should be updated
                    entitiesToUpdate.Add(entity);
                }
            }

            // Start a transaction to ensure data integrity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // BulkInsert for new entities
                    if (entitiesToInsert.Any())
                    {
                        await _context.BulkInsertAsync(entitiesToInsert, new BulkConfig
                        {
                            UseTempDB = true,
                            BatchSize = entitiesToInsert.Count,
                            SetOutputIdentity = true  // Get the generated Ids for inserted entities
                        });
                    }

                    // BulkUpdate for existing entities
                    if (entitiesToUpdate.Any())
                    {
                        await _context.BulkUpdateAsync(entitiesToUpdate, new BulkConfig
                        {
                            UseTempDB = true,
                            BatchSize = entitiesToUpdate.Count
                        });
                    }

                    // Commit transaction after both insert and update are done
                    await transaction.CommitAsync();
                    return Ok(new { Inserted = entitiesToInsert, Updated = entitiesToUpdate });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }

        [HttpPost("BulkDelete")]
        public async Task<IActionResult> BulkDelete([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("The list of IDs cannot be empty.");
            }

            // Start a transaction to ensure data integrity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Retrieve the entities that match the given IDs
                    var entitiesToDelete = await _dbSet.Where(e => ids.Contains(e.Id)).ToListAsync();

                    if (!entitiesToDelete.Any())
                    {
                        return NotFound("No entities found for the provided IDs.");
                    }

                    // Bulk delete the entities
                    await _context.BulkDeleteAsync(entitiesToDelete, new BulkConfig
                    {
                        UseTempDB = true,
                        BatchSize = entitiesToDelete.Count
                    });

                    // Commit transaction
                    await transaction.CommitAsync();

                    return Ok(new { Deleted = entitiesToDelete });
                }
                catch (Exception ex)
                {
                    // Rollback transaction in case of an error
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }

        //Lưu ý đối với những model có tham chiếu lặp lại giữa các đối tượng,
        //chẳng hạn như đối tượng Manager trong Model User tự tham chiếu hoặc có quan hệ phức tạp với nhau trong nhiều cấp.
        //Khi sử dụng hàm CheckDataIsExist, nên thêm Property [JsonIgnore] trước đối tượng đó. Ví dụ Manager thì thêm [JsonIgnore] trước thuộc tính Manager.



    }



    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : DynamicCustomController<User>
    {
        public UsersController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : DynamicCustomController<Account>
    {
        public AccountsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class StoreRecordsController : DynamicCustomController<StoreRecord>
    {
        public StoreRecordsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplatesController : DynamicCustomController<EmailTemplate>
    {
        public EmailTemplatesController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTrackingsController : DynamicCustomController<EmailTracking>
    {
        public EmailTrackingsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestListsController : DynamicCustomController<EmailTestList>
    {
        public EmailTestListsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : DynamicCustomController<Roles>
    {
        public RolesController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : DynamicCustomController<Permission>
    {
        public PermissionsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : DynamicCustomController<RolePermission>
    {
        public RolePermissionsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : DynamicCustomController<Test>
    {
        public TestsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class TestDetailsController : DynamicCustomController<TestDetail>
    {
        public TestDetailsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class MessageBoxController : DynamicCustomController<MessageBox>
    {
        public MessageBoxController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : DynamicCustomController<Messages>
    {
        public MessagesController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : DynamicCustomController<Job>
    {
        public JobsController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : DynamicCustomController<Bookmarks>
    {
        public BookmarksController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class CriteriasController : DynamicCustomController<Criterias>
    {
        public CriteriasController(AppDbContext context)
            : base(context) { }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : DynamicCustomController<JobApplications>
    {
        public JobApplicationsController(AppDbContext context)
            : base(context) { }
    }

}
