using src.Dtos.Email;
using src.Models;

namespace src.Repositories.Email
{
    public interface IEmailRepository
    {
        // 1. Luôn phải có ToEmails và Subject
        // 2. Phải có Body hoặc [TemplaeId và Replacements] mới gửi được (Nếu có cả 2 thì ưu tiên Body)
        // 3. Các thuộc tính khác có hay không cũng được
        // 4. Priority nếu không có mặc định là Normal (có 3 mức: High, Normal, Low)
        Task<(bool Success, string ErrorMessage)> SendEmailAsync(
        string toEmails,
        string subject,
        string? body = null,
        string? templateId = null,
        Dictionary<string, string>? replacements = null,
        List<EmailAttachmentInfo>? attachments = null,
        string? ccEmails = null,
        string? bccEmails = null,
        string? priority = null,
        string? smtpAddress = null,
        int? portNumber = null,
        string? emailFrom = null,
        string? password = null,
        bool? isTest = true,
        string? refId = null,
        string? dataSource = null
    );

        Task<EmailTemplate?> GetTemplateByIdAsync(string templateId);
    }
}
