using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class BookmarksConfig : IEntityTypeConfiguration<Bookmarks>
    {
        public void Configure(EntityTypeBuilder<Bookmarks> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Job)
                   .WithMany()
                   .HasForeignKey(x => x.JobId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                new Bookmarks { Id = 1, UserId = 1, JobId = 1 },
                new Bookmarks { Id = 2, UserId = 1, JobId = 2 },
                new Bookmarks { Id = 3, UserId = 1, JobId = 3 },
                new Bookmarks { Id = 4, UserId = 1, JobId = 4 },
                new Bookmarks { Id = 5, UserId = 3, JobId = 1 },
                new Bookmarks { Id = 6, UserId = 2, JobId = 2 },
                new Bookmarks { Id = 7, UserId = 2, JobId = 3 },
                new Bookmarks { Id = 8, UserId = 2, JobId = 4 }
            );
        }
    }
}