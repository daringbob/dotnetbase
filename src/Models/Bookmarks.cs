using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Bookmarks : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


}
