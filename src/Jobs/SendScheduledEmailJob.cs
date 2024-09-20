using src.Repositories.Email;
using src.Models;
using src.Dtos.Email;
public class SendScheduledEmailJob
{
    private readonly IEmailRepository _emailRepository;

    public SendScheduledEmailJob(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public async Task SendScheduledEmailAsync()
    {
        // Lấy thời gian hiện tại và định dạng nó
        var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Tạo danh sách đính kèm
        var attachments = new List<EmailAttachmentInfo>
        {
            new EmailAttachmentInfo
            {
                Base64 = "YWFhYQ==", // Nội dung file base64
                FileName = "text.txt" // Tên file đính kèm
            }
        };

        // Gọi phương thức SendEmailAsync từ IEmailRepository
        var (success, errorMessage) = await _emailRepository.SendEmailAsync(
            "leluthiennhan31@gmail.com",
            "Test Scheduled Email",
            null, // không xài body
            "ET-002",
            new Dictionary<string, string>
            {
                { "Name", "Nhan" },
                { "Time", currentTime },
                { "Message", "Tin nhắn tự động ET-002" }
            },
            attachments
        );

        //Gọi phương thức với tên tham số để không cần truyền null cho các tham số không sử dụng
        /*var (success, errorMessage) = await _emailRepository.SendEmailAsync(
            toEmails: "lltnhan@cmctssg.info",
            subject: "Test Scheduled Email",
            body: "Đây là email được gửi tự động theo lịch",
            priority: "High"
        );*/

        if (!success)
        {
            throw new Exception($"Error sending email: {errorMessage}");
        }
    }
}
