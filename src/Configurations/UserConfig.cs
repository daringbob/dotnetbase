using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder
                .HasOne(l => l.Roles)
                .WithMany()
                .HasForeignKey(l => l.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            // init data
            builder.HasData(
                new User
                {
                    Id = 1,
                    Code = "AD001",
                    FirstName = "ADMIN",
                    LastName = "SYS",
                    FullName = "ADMIN SYS",
                    IsActive = true,
                    IsInputInformation = true,
                }
            );

        }
    }
}
