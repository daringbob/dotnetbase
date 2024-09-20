using System.Net.Mail;
using src.Models;
using Microsoft.Extensions.Configuration;
namespace src.Dtos.Email
{
//Định nghĩa các thuộc tính nhận dữ liệu từ api gửi mail 
    public class EmailRequest
    {
        public string ToEmails { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public string? TemplateId { get; set; }
        public Dictionary<string, string>? Replacements { get; set; }
        public List<EmailAttachmentInfo>? Attachments { get; set; }
        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string? Priority { get; set; }
        public string? SmtpAddress { get; set; }
        public int? PortNumber { get; set; }
        public string? EmailFrom { get; set; }
        public string? Password { get; set; }

        //các thuộc tính bổ sung cho mail tracking
        public bool? IsTest { get; set; }// Mail có phải là mail test không
        public string? RefId { get; set; } // RefId của table báo cáo
        public string? DataSource { get; set; } // Source báo cáo
        
        // Xóa bỏ hàm khởi tạo có tham số
    }

}
