using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.OpenApi.Extensions;
using src.Models.@enum;

namespace src.Models
{
    public class Roles : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }

        public string Status { get; set; } = StatusEnum.Activated.GetDisplayName();

        public int Level { get; set; } = 2;
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
