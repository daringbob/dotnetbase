using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasOne(x => x.Recruiter).WithMany().HasForeignKey(x => x.RecruiterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Bookmarks).WithOne(x => x.Job).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.HasData(
                new Job
                {
                    Id = 1,
                    Locations = "Hồ Chí Minh",
                    MinSalary = 10000000,
                    MaxSalary = 20000000,
                    Description = "Develop and maintain software applications.",
                    Requirement = "C#, .NET, SQL",
                    WorkingModelId = 11,
                    JobTypeId = 2,
                    ExperienceId = 3,
                    JobTitleId = 1,
                    RecruiterId = 1,
                },
                 new Job
                 {
                     Id = 2,
                     Locations = "NewYork",
                     MinSalary = 20000000,
                     MaxSalary = 30000000,
                     Description = "Develop and maintain software applications.",
                     Requirement = "C#, .NET, SQL",
                     WorkingModelId = 12,
                     JobTypeId = 8,
                     ExperienceId = 4,
                     JobTitleId = 6,
                     RecruiterId = 2,
                 },
                 new Job
                 {
                     Id = 3,
                     Locations = "Hồ Chí Minh",
                     MinSalary = 30000000,
                     MaxSalary = 40000000,
                     Description = "Develop and maintain software applications.",
                     Requirement = "C#, .NET, SQL",
                     WorkingModelId = 13,
                     JobTypeId = 9,
                     ExperienceId = 5,
                     JobTitleId = 7,
                     RecruiterId = 2,
                 },
                 new Job
                 {
                     Id = 4,
                     Locations = "Hồ Chí Minh",
                     MinSalary = 12000000,
                     MaxSalary = 13000000,
                     Description = "Develop and maintain software applications.",
                     Requirement = "C#, .NET, SQL",
                     WorkingModelId = 11,
                     JobTypeId = 9,
                     ExperienceId = 10,
                     JobTitleId = 7,
                     RecruiterId = 1,
                 }
            );
        }
    }
}
