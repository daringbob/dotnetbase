using Bogus;
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

            var bookmarksFaker = new Faker<Bookmarks>()
                .RuleFor(x => x.Id, f => f.IndexFaker + 1)  // Id ngẫu nhiên
                                                            // .RuleFor(x => x.Id, f => f.PickRandom(1, 2, 3))  // Id ngẫu nhiên
                .RuleFor(x => x.UserId, 2)
                .RuleFor(x => x.JobId, f => f.IndexFaker + 1);

            // Tạo dữ liệu mẫu với vòng lặp
            var Bookmarks = bookmarksFaker.Generate(20);
            builder.HasData(Bookmarks);  // Đưa dữ liệu vào DBContext
        }
    }
}