using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class TestDetailConfig : IEntityTypeConfiguration<TestDetail>
    {
        public void Configure(EntityTypeBuilder<TestDetail> builder)
        {
            // init data, admin:admin
            builder.HasOne(x => x.TestA)
                .WithMany()
                .HasForeignKey(x => x.TestAId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
