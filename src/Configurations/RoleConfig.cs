using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OpenApi.Extensions;
using src.Models;
using src.Models.@enum;

namespace src.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            // init data, admin:admin
            builder.HasData(

                new Roles
                {
                    Id = 1,
                    Title = "Admin",
                    Status = StatusEnum.Activated.GetDisplayName()
                },
                new Roles
                {
                    Id = 2,
                    Title = "User",
                    Status = StatusEnum.Activated.GetDisplayName()
                }
            );
        }
    }
}
