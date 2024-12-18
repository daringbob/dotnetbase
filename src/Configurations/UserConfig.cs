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
                    UserType = "Recruiter",
                    Gender = "Male",
                    GeoLocation = "{latitude: 21.028511,longitude: 105.804817, latitudeDelta: 0.05,longitudeDelta: 0.05}"
                },
                new User
                {
                    Id = 2,
                    Code = "AD002",
                    FirstName = "ADMIN 002",
                    LastName = "SYS 002",
                    FullName = "ADMIN SYS 002",
                    IsActive = true,
                    IsInputInformation = true,
                    UserType = "Candidate",
                    Gender = "Female"
                },
                new User
                {
                    Id = 3,
                    Code = "Recruiter001",
                    FirstName = "Recruiter",
                    LastName = "003",
                    FullName = "Recruiter001",
                    IsActive = true,
                    IsInputInformation = true,
                    UserType = "Recruiter",
                    Gender = "Female",
                    GeoLocation = "{latitude: 23.10598, longitude: 72.926834, latitudeDelta: 0.05, longitudeDelta: 0.05}"
                }
            );

        }
    }
}
