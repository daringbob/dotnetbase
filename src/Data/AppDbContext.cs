using System.Reflection;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Triggers;

namespace src.Data
{
    public class AppDbContext : DbContext
    {
        //GENERAL

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<StoreRecord> StoreRecords { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailTracking> EmailTrackings { get; set; }
        public DbSet<EmailTestList> EmailTestLists { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }

        public DbSet<MessageBox> MessageBox { get; set; }
        public DbSet<Messages> Messages { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Bookmarks> Bookmarks { get; set; }

        public DbSet<Criterias> Criterias { get; set; }

        public DbSet<JobApplications> JobApplications { get; set; }

        private ExecuteTriggerEvent HandleTriggers()
        {
            var auditableTrigger = new AuditableTrigger(this);
            var userTrigger = new UserTrigger(this);

            return new ExecuteTriggerEvent
            {
                BeforeSave =
                [
                    // Run before save
                    auditableTrigger.ExecuteBeforeSave,
                    userTrigger.ExecuteBeforeSave
                ],
                Saved =
                [
                    // Run after save
                    auditableTrigger.ExecuteSaved,
                    userTrigger.ExecuteSaved
                ]
            };
        }

        private void ProcessAdditionalOperations(List<Func<Task>> operations)
        {
            foreach (var operation in operations)
            {
                operation().GetAwaiter().GetResult();
            }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ApplyGlobalConfigurations(modelBuilder);
        }

        public override int SaveChanges()
        {
            var handler = HandleTriggers();
            ProcessAdditionalOperations(handler.BeforeSave);

            var result = base.SaveChanges();

            ProcessAdditionalOperations(handler.Saved);

            return result;
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            var handler = HandleTriggers();
            ProcessAdditionalOperations(handler.BeforeSave);

            var result = await base.SaveChangesAsync(cancellationToken);

            ProcessAdditionalOperations(handler.Saved);

            return result;
        }

        private void ApplyGlobalConfigurations(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.Created))
                        .IsRequired()
                        .HasDefaultValueSql("GETDATE()");

                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.Modified))
                        .IsRequired()
                        .HasDefaultValueSql("GETDATE()");
                }
            }
        }
    }

    public class ExecuteTriggerEvent
    {
        public List<Func<Task>> BeforeSave { get; set; } = new List<Func<Task>>();
        public List<Func<Task>> Saved { get; set; } = new List<Func<Task>>();
    }
}
