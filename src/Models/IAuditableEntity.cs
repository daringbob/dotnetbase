namespace src.Models
{
    interface IAuditableEntity
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
