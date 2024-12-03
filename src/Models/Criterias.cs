using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Criterias : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string CriteriaType { get; set; }

        public required string Title { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }


    }


}
