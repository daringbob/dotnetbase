using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class MessageBoxConfig : IEntityTypeConfiguration<MessageBox>
    {
        public void Configure(EntityTypeBuilder<MessageBox> builder)
        {
            builder.HasOne(x => x.Candidate)
                .WithMany()
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Recruiter)
                .WithMany()
                .HasForeignKey(x => x.RecruiterId)
                .OnDelete(DeleteBehavior.NoAction);

            // init data, admin:admin
            builder.HasData(
                new MessageBox
                {
                    Id = 1,
                    CandidateId = 2,
                    RecruiterId = 1,
                    LastMessageId = 2
                }
            );
        }
    }
}
