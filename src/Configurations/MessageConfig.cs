using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class MessagesConfig : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

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
                new Messages
                {
                    Id = 1,
                    SenderId = 1,
                    CandidateId = 2,
                    RecruiterId = 1,
                    Message = "Hello",
                    JobId = 1
                },
                new Messages
                {
                    Id = 2,
                    SenderId = 2,
                    CandidateId = 2,
                    RecruiterId = 1,
                    Message = "Hi",
                    JobId = 1
                },
                new Messages
                {
                    Id = 3,
                    SenderId = 2,
                    CandidateId = 2,
                    RecruiterId = 1,
                    Message = "ADMIN SYS has accepted the job application.",
                    IsSystemMessage = true,
                    JobId = 1
                }
            );
        }
    }
}
