using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class TestDetail : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string? TestDetailString { get; set; }

        [ForeignKey("TestA")]
        public int? TestAId { get; set; }
        public Test? TestA { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

}
