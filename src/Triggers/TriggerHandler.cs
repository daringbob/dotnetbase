using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using src.Data;
using src.Models;

namespace src.Triggers
{
    public class TriggerHandler<T> : ITriggerHandler<T>
        where T : class
    {
        public readonly AppDbContext _context;
        public readonly List<EntityEntry> _entries;
        public readonly List<PropertyValues> _originalEntries = new();

        public TriggerHandler(AppDbContext context)
        {
            _context = context;

            var entries = context
                .ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is T
                    && (e.State == EntityState.Added || e.State == EntityState.Modified)
                )
                .ToList();
            _entries = entries;

            // Capture the original values for modified entities
            foreach (var entry in _entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    _originalEntries.Add(entry.OriginalValues.Clone());
                }
            }
        }

        public virtual async Task ExecuteSaved()
        {
            if (_entries.Count() > 0)
            {
                foreach (var entityEntry in _entries)
                {
                    var entity = (T)entityEntry.Entity;

                    OnCreatedOrModified(entity);
                    await OnCreatedOrModifiedAsync(entity);

                    if (entityEntry.State == EntityState.Added)
                    {
                        OnCreated(entity);
                        await OnCreatedAsync(entity);
                    }
                }
            }
        }

        public virtual async Task ExecuteBeforeSave()
        {
            if (_entries.Count() > 0)
            {
                foreach (var entityEntry in _entries)
                {
                    var entity = (T)entityEntry.Entity;

                    BeforeCreateOrModify(entity);
                    await BeforeCreateOrModifyAsync(entity);

                    if (entityEntry.State == EntityState.Added)
                    {
                        BeforeCreate(entity);
                        await BeforeCreateAsync(entity);
                    }
                }
            }
        }

        public virtual void OnCreated(T entity) { }

        public virtual void OnCreatedOrModified(T entity) { }

        public virtual Task OnCreatedAsync(T entity)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnCreatedOrModifiedAsync(T entity)
        {
            return Task.CompletedTask;
        }

        public virtual void BeforeCreate(T entity) { }

        public virtual void BeforeCreateOrModify(T entity) { }

        public virtual Task BeforeCreateAsync(T entity)
        {
            return Task.CompletedTask;
        }

        public virtual Task BeforeCreateOrModifyAsync(T entity)
        {
            return Task.CompletedTask;
        }
    }

    public interface ITriggerHandler<T>
    {
        Task ExecuteSaved();
        Task ExecuteBeforeSave();
        void OnCreated(T entity);
        void OnCreatedOrModified(T entity);
        Task OnCreatedAsync(T entity);
        Task OnCreatedOrModifiedAsync(T entity);
        void BeforeCreate(T entity);
        void BeforeCreateOrModify(T entity);
        Task BeforeCreateAsync(T entity);
        Task BeforeCreateOrModifyAsync(T entity);
    }
}
