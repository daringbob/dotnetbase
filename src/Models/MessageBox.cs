using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class MessageBox : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
       
       public int CandidateId { get; set; }
         public int RecruiterId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    
}
