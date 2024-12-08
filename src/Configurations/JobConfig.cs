using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;
using Bogus;  // Thư viện Faker giả lập dữ liệu ngẫu nhiên

namespace src.Configurations
{
    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasOne(x => x.Recruiter).WithMany().HasForeignKey(x => x.RecruiterId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Bookmarks).WithOne(x => x.Job).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.JobApplications).WithOne(x => x.Jobs).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            // Sử dụng Bogus để tạo dữ liệu ngẫu nhiên
            var jobFaker = new Faker<Job>()
                .RuleFor(x => x.Id, f => f.IndexFaker + 1)  // Id ngẫu nhiên
                                                            // .RuleFor(x => x.Id, f => f.PickRandom(1, 2, 3))  // Id ngẫu nhiên
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())  // Tiêu đề công việc ngẫu nhiên
                .RuleFor(x => x.Deadline, f => f.Date.Future())  // Hạn nộp hồ sơ ngẫu nhiên
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))  // Số lượng tuyển ngẫu nhiên
                .RuleFor(x => x.WorkingHours, f => f.Lorem.Sentence())  // Thời gian làm việc ngẫu nhiên
                .RuleFor(x => x.Gender, f => f.PickRandom(new[] { "Male", "Female", "No require" }))  // Giới tính ngẫu nhiên
                .RuleFor(x => x.Interest, f => f.Lorem.Sentence())  // Quyền lợi ngẫu nhiên


                .RuleFor(x => x.MinSalary, f => f.Random.Int(1000000, 20000000))  // Mức lương tối thiểu ngẫu nhiên
                .RuleFor(x => x.MaxSalary, f => f.Random.Int(20000000, 50000000))  // Mức lương tối đa ngẫu nhiên
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())  // Mô tả công việc ngẫu nhiên
                .RuleFor(x => x.Requirement, f => f.Lorem.Words(5).ToString())  // Yêu cầu ngẫu nhiên
                .RuleFor(x => x.WorkingModelId, f => f.PickRandom(new[] { 11, 12, 13 }))  // Working model id ngẫu nhiên
                .RuleFor(x => x.JobTypeId, f => f.PickRandom(new[] { 2, 8, 9 }))
                .RuleFor(x => x.ExperienceId, f => f.PickRandom(new[] { 3, 4, 5, 10 }))  // Experience id ngẫu nhiên
                .RuleFor(x => x.JobTitleId, f => f.PickRandom(new[] { 1, 6, 7 }))  // Job title id ngẫu nhiên
                .RuleFor(x => x.RecruiterId, f => f.PickRandom(new[] { 1, 3 }))  // Recruiter id ngẫu nhiên;  
                .RuleFor(x => x.IsActive, f => true); // Trạng thái công việc ngẫu nhiên


            // Tạo dữ liệu mẫu với vòng lặp
            var jobs = jobFaker.Generate(100);
            builder.HasData(jobs);  // Đưa dữ liệu vào DBContext
        }
    }
}
