using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations {
    public class CriteriasConfig  : IEntityTypeConfiguration<Criterias>{
        public void Configure(EntityTypeBuilder<Criterias> builder)
        {
            builder.HasData(
    new Criterias
    {
        Id = 1,
        CriteriaType = "JobTitle",
        Title = "Software Engineer",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 2,
        CriteriaType = "JobType",
        Title = "Full-time",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 3,
        CriteriaType = "Experience",
        Title = "2 years",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 4,
        CriteriaType = "Experience",
        Title = "3 years",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 5,
        CriteriaType = "Experience",
        Title = "5 years",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 6,
        CriteriaType = "JobTitle",
        Title = "Data Scientist",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 7,
        CriteriaType = "JobTitle",
        Title = "Project Manager",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 8,
        CriteriaType = "JobType",
        Title = "Part-time",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 9,
        CriteriaType = "JobType",
        Title = "Contract",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 10,
        CriteriaType = "Experience",
        Title = "10 years",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 11,
        CriteriaType = "WorkingModel",
        Title = "Remote",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 12,
        CriteriaType = "WorkingModel",
        Title = "Hybrid",
        Created = DateTime.Now,
        Modified = DateTime.Now
    },
    new Criterias
    {
        Id = 13,
        CriteriaType = "WorkingModel",
        Title = "Onsite",
        Created = DateTime.Now,
        Modified = DateTime.Now
    }
);

        }

    }
}