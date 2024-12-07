using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class JobApplicationsConfig : IEntityTypeConfiguration<JobApplications>
    {
        public void Configure(EntityTypeBuilder<JobApplications> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Khởi tạo dữ liệu (nếu cần)
            // Ví dụ: thêm các cấu hình mặc định cho bảng
        }
    }
}
