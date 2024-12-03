using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using src.Models;

namespace src.Configurations
{
    public class MessagesConfig : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {

            // init data, admin:admin
            builder.HasData(
                new Messages
                {
                    Id = 1,
                    SenderId = 1,
                    ReceiverId = 2,
                    Message = "Hello"
                },
                new Messages
                {
                    Id = 2,
                    SenderId = 2,
                    ReceiverId = 1,
                    Message = "Hi"
                }
            );
        }
    }
}
