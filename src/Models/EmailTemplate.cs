

namespace src.Models
{
    public class EmailTemplate : IAuditableEntity
    {
        public int Id { get; set; }
        public required string TemplateId { get; set; }
        public required string Template { get; set; }
        public string? DefaultToEmails { get; set; } 
        public string? DefaultCcEmails { get; set; } 
        public string? DefaultBccEmails { get; set; } 
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
