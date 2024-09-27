using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class StoreRecord : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int? RefID { get; set; }
        public int? ParentID { get; set; }
        public string? DataSource { get; set; }
        public required string ServerRelativeUrl { get; set; }
        public string? downloadUrl { get; set; }
        public bool IsFolder { get; set; } = false;
    }
}