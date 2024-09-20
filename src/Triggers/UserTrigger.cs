using src.Data;
using src.Models;

namespace src.Triggers
{
    class UserTrigger(AppDbContext context) : TriggerHandler<User>(context)
    {
        // After defining, add the trigger to AppDbContext
        public override Task OnCreatedOrModifiedAsync(User record)
        {
            var previous = _originalEntries.FirstOrDefault(e => (int)e?["Id"]! == record.Id);
            var originalIsActive = previous?.GetValue<bool?>("IsActive");
            var currentIsActive = record.IsActive;

            if (originalIsActive != currentIsActive)
            {
                var accountList = _context.Accounts.Where(a => a.UserId == record.Id).ToList();
                foreach (var account in accountList)
                {
                    account.IsActive = record.IsActive;
                }

                _context.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
