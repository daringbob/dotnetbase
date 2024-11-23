using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Test : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }


        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

}
