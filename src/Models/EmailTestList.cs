

namespace src.Models
{
    public class EmailTestList : IAuditableEntity
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
