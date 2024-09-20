using src.Models;
namespace src.Dtos.Email
{
    // 1 kiểu dữ liệu mới để quản lý file đính kèm trong mail
    public class EmailAttachmentInfo
    {
        public required string Base64 { get; set; } // Nội dung file dạng base64
        public required string FileName { get; set; } // Tên file
    }
}
