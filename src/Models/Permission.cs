using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.OpenApi.Extensions;
using src.Models.@enum;

namespace src.Models
{
    public class Permission : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; } // Tên của vai trò, ví dụ: "Admin", "User"
        public string Status { get; set; } = StatusEnum.Activated.GetDisplayName();

        // Tạo mối quan hệ nhiều-nhiều với bảng Permission thông qua bảng RolePermission
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
