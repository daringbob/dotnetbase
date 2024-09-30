using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class StoreRecordConfig : IEntityTypeConfiguration<StoreRecord>
    {
        public void Configure(EntityTypeBuilder<StoreRecord> builder)
        {

            builder.HasData(
                new StoreRecord
                {
                    Id = 1,
                    Name = "ImageStore",
                    ServerRelativeUrl = "/ImageStore",
                    IsFolder = true
                },
                new StoreRecord
                {
                    Id = 2,
                    Name = "DocumentStore",
                    ServerRelativeUrl = "/DocumentStore",
                    IsFolder = true
                }
            );

        }
    }
}
