using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // init data, admin:admin
            builder.HasData(
                new Account
                {
                    Id = 1,
                    Password =
                        "66AF45DD053BF5204A28D19564B9CFD37DCB0EA6AB6E6F212CDFBC0A6A833EA27489FD432565F8AD7ECB816CCF5D8000B9B72C61A5ED9D4BEF184886F6DE36F4",
                    UserId = 1,
                    UserName = "admin",
                    AccountType = AccountType.User,
                    IsActive = true
                },
                new Account
                {
                    Id = 2,
                    Password =
                        "66AF45DD053BF5204A28D19564B9CFD37DCB0EA6AB6E6F212CDFBC0A6A833EA27489FD432565F8AD7ECB816CCF5D8000B9B72C61A5ED9D4BEF184886F6DE36F4",
                    UserId = 2,
                    UserName = "admin002",
                    AccountType = AccountType.User,
                    IsActive = true
                },
                new Account
                {
                    Id = 3,
                    Password =
                        "66AF45DD053BF5204A28D19564B9CFD37DCB0EA6AB6E6F212CDFBC0A6A833EA27489FD432565F8AD7ECB816CCF5D8000B9B72C61A5ED9D4BEF184886F6DE36F4",
                    UserId = 3,
                    UserName = "recruiter001",
                    AccountType = AccountType.User,
                    IsActive = true
                }
            );
        }
    }
}
