using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasData(
                new Job
                {
                    Id = 1,
                    Locations = "Hồ Chí Minh",
                    MinSalary = 10000000,
                    MaxSalary = 20000000,
                    Description = "Develop and maintain software applications.",
                    Requirement = "C#, .NET, SQL",
                    JobTypeId = 2,
                    ExperienceId = 2,
                    JobTitleId = 1
                }
            );
        }
    }
}
