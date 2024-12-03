using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class CriteriasConfig : IEntityTypeConfiguration<Criterias>
    {
        public void Configure(EntityTypeBuilder<Criterias> builder)
        {
            builder.HasData(
    new Criterias
    {
        Id = 1,
        CriteriaType = "JobTitle",
        Title = "Software Engineer",
    },
    new Criterias
    {
        Id = 2,
        CriteriaType = "JobType",
        Title = "Full-time",
    },
    new Criterias
    {
        Id = 3,
        CriteriaType = "Experience",
        Title = "2 years",
    },
    new Criterias
    {
        Id = 4,
        CriteriaType = "Experience",
        Title = "3 years",
    },
    new Criterias
    {
        Id = 5,
        CriteriaType = "Experience",
        Title = "5 years",
    },
    new Criterias
    {
        Id = 6,
        CriteriaType = "JobTitle",
        Title = "Data Scientist",
    },
    new Criterias
    {
        Id = 7,
        CriteriaType = "JobTitle",
        Title = "Project Manager",
    },
    new Criterias
    {
        Id = 8,
        CriteriaType = "JobType",
        Title = "Part-time",
    },
    new Criterias
    {
        Id = 9,
        CriteriaType = "JobType",
        Title = "Contract",
    },
    new Criterias
    {
        Id = 10,
        CriteriaType = "Experience",
        Title = "10 years",
    },
    new Criterias
    {
        Id = 11,
        CriteriaType = "WorkingModel",
        Title = "Remote",
    },
    new Criterias
    {
        Id = 12,
        CriteriaType = "WorkingModel",
        Title = "Hybrid",
    },
    new Criterias
    {
        Id = 13,
        CriteriaType = "WorkingModel",
        Title = "Onsite",
    }
);

        }

    }
}