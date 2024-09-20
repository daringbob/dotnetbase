namespace src.Models
{
    public class EmailTracking : IAuditableEntity
    {
        public int Id { get; set; }
        public required string EmailFrom { get; set; }
        public required string ToEmails { get; set; } // Danh sách email người nhận, ngăn cách bởi dấu chấm phẩy
        public required string Subject { get; set; }
        public required string Body { get; set; } // Body Email nếu không xài template
        public required string TemplateId { get; set; } // TemplateId nếu xài Template làm body

        public string? CcEmails { get; set; } // Chuỗi CC Emails ngăn cách bởi dấu chấm phẩy
        public string? BccEmails { get; set; } // Chuỗi BCC Emails ngăn cách bởi dấu chấm phẩy
        public string? Priority { get; set; } // Mức độ ưu tiên, mặc định là bình thường
        public bool? IsFailed { get; set; }
        public string? FailedContent { get; set; }
        public bool? IsTest { get; set; }// Mail có phải là mail test không
        public string? RefId { get; set; } // RefId của table báo cáo
        public string? DataSource { get; set; } // Source báo cáo
        public bool? HasAttachment { get; set; } // Source báo cáo
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
