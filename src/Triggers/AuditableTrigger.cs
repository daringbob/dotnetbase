using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using src.Data;
using src.Models;

namespace src.Triggers
{
    class AuditableTrigger(AppDbContext context) : TriggerHandler<IAuditableEntity>(context)
    {
        public override void BeforeCreateOrModify(IAuditableEntity record)
        {
            record.Modified = DateTime.Now;
        }

        public override void BeforeCreate(IAuditableEntity record)
        {
            record.Created = DateTime.Now;
        }
    }
}
