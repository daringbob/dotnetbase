using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;

namespace src.Controllers
{
    [Authorize]
    public class DynamicODataController<TEntity> : ODataController
        where TEntity : class
    {
        public readonly AppDbContext _context;
        public readonly DbSet<TEntity> _dbSet;

        public DynamicODataController(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        [EnableQuery(PageSize = 2000)]
        public virtual IActionResult Get()
        {
            return Ok(_dbSet);
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            var entity = _dbSet.Find(key);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return Created(entity);
        }

        public async Task<IActionResult> Patch(
            [FromODataUri] int key,
            [FromBody] Delta<TEntity> delta
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _dbSet.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            delta.Patch(entity);
            await _context.SaveChangesAsync();
            return Updated(entity);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var entity = await _dbSet.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class AccountsController : DynamicODataController<Account>
    {
        public AccountsController(AppDbContext context)
            : base(context) { }
    }
    public class StoreRecordsController : DynamicODataController<StoreRecord>
    {
        public StoreRecordsController(AppDbContext context)
            : base(context) { }
    }

    public class EmailTemplatesController : DynamicODataController<EmailTemplate>
    {
        public EmailTemplatesController(AppDbContext context)
            : base(context) { }
    }
    public class EmailTrackingsController : DynamicODataController<EmailTracking>
    {
        public EmailTrackingsController(AppDbContext context)
            : base(context) { }
    }
    public class EmailTestListsController : DynamicODataController<EmailTestList>
    {
        public EmailTestListsController(AppDbContext context)
            : base(context) { }
    }

    public class RolesController : DynamicODataController<Roles>
    {
        public RolesController(AppDbContext context)
            : base(context) { }
    }

    public class PermissionsController : DynamicODataController<Permission>
    {
        public PermissionsController(AppDbContext context)
            : base(context) { }
    }

    public class RolePermissionsController : DynamicODataController<RolePermission>
    {
        public RolePermissionsController(AppDbContext context)
            : base(context) { }
    }
    public class TestsController : DynamicODataController<Test>
    {
        public TestsController(AppDbContext context)
            : base(context) { }
    }

    public class TestDetailsController : DynamicODataController<TestDetail>
    {
        public TestDetailsController(AppDbContext context)
            : base(context) { }
    }

    public class MessageBoxController : DynamicODataController<MessageBox>
    {
        public MessageBoxController(AppDbContext context)
            : base(context) { }
    }

    public class MessagesController : DynamicODataController<Messages>
    {
        public MessagesController(AppDbContext context)
            : base(context) { }
    }

    public class JobsController : DynamicODataController<Job>
    {
        public JobsController(AppDbContext context)
            : base(context) { }
    }

    public class BookmarksController : DynamicODataController<Bookmarks>
    {
        public BookmarksController(AppDbContext context)
            : base(context) { }
    }

    public class CriteriasController : DynamicODataController<Criterias>
    {
        public CriteriasController(AppDbContext context)
            : base(context) { }
    }

    public class JobApplicationsController : DynamicODataController<JobApplications>
    {
        public JobApplicationsController(AppDbContext context)
            : base(context) { }
    }

    public class UsersController : DynamicODataController<Users>
    {
        public UsersController(AppDbContext context)
            : base(context) { }
    }


    

}
