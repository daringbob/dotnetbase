using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.OpenApi.Extensions;
using src.Models.@enum;

namespace src.Models
{
    public class RolePermission : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Roles? Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public bool IsCheck { get; set; } = false;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
