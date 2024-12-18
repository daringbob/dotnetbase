using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Job : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; } // Tiêu đề bài đăng
        public required double MinSalary { get; set; }
        public required double MaxSalary { get; set; }

        public required string Description { get; set; }

        public required string Requirement { get; set; }

        [InverseProperty("Jobs")]
        public List<JobApplications>? JobApplications { get; set; }


        [InverseProperty("Job")]
        public List<Bookmarks>? Bookmarks { get; set; }

        [ForeignKey("WorkingModel")]
        public int? WorkingModelId { get; set; }
        public Criterias? WorkingModel { get; set; }

        [ForeignKey("JobType")]
        public int? JobTypeId { get; set; }
        public Criterias? JobType { get; set; }
        [ForeignKey("Experience")]
        public int? ExperienceId { get; set; }
        public Criterias? Experience { get; set; }
        [ForeignKey("JobTitle")]
        public int? JobTitleId { get; set; }
        public Criterias? JobTitle { get; set; }
        [ForeignKey("Recruiter")]
        public int RecruiterId { get; set; }
        public User? Recruiter { get; set; }

        public Boolean IsActive { get; set; } = true;

        // Hạn nộp
        public DateTime? Deadline { get; set; }
        // Số lượng tuyển
        public int? Quantity { get; set; }
        // Thời gian làm việc
        public string? WorkingHours { get; set; }
        // Giới tính
        public string? Gender { get; set; }

        // Quyền lợi
        public string? Interest { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


}
