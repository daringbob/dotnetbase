using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Dtos.Email;
using src.Models;

namespace src.Repositories.Email
{
    public class EmailRepository : IEmailRepository
    {
         private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //Hàm thay thế các biến vô template thành body email hoàn chỉnh
        private static string ApplyTemplate(string template, Dictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                template = template.Replace("[[" + replacement.Key + "]]", replacement.Value);
            }
            return template;
        }

        // Hàm để gộp các email nhận vào từ hàm với email mặc định của tmeplate và loại bỏ trùng lặp
        private static string CombineEmails(string? inputEmails, string? defaultEmails)
        {
            HashSet<string> emailSet = new HashSet<string>();

            if (!string.IsNullOrEmpty(inputEmails))
            {
                foreach (var email in ParseEmails(inputEmails))
                {
                    emailSet.Add(email);
                }
            }

            if (!string.IsNullOrEmpty(defaultEmails))
            {
                foreach (var email in ParseEmails(defaultEmails))
                {
                    emailSet.Add(email);
                }
            }

            return string.Join(';', emailSet);
        }

        //Hàm loại bỏ ; trong chuỗi emial để tạo list các emails cần gửi
        private static List<string> ParseEmails(string emails)
        {
            return emails.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        //Hàm lưu thông tin vào EmailTracking
        private async Task TrackEmailAsync(
            string emailFrom,
            string toEmails,
            string subject,
            string body,
            string? templateId,
            string? ccEmails = null,
            string? bccEmails = null,
            string? priority = null,
            bool? isFailed = false,
            string? failedContent = null,
            bool? isTest = true,
            string? refId = null,
            string? dataSource = null,
            bool? hasAttachment = false
        )
        {
            var emailTracking = new EmailTracking
            {
                EmailFrom = emailFrom,
                ToEmails = toEmails,
                Subject = subject,
                Body = body,
                TemplateId = templateId ?? "N/A", // Sử dụng "N/A" nếu không dùng template
                CcEmails = ccEmails,
                BccEmails = bccEmails,
                Priority = priority ?? "Normal", // Mặc định là "Normal" nếu không có giá trị
                IsFailed = isFailed, // Ghi lại trạng thái thất bại
                FailedContent = failedContent, // Ghi lại nội dung lỗi nếu có
                IsTest = isTest,
                RefId = refId,
                DataSource = dataSource,
                HasAttachment = hasAttachment 
            };

            _context.EmailTrackings.Add(emailTracking);
            await _context.SaveChangesAsync();
        }

        //Hàm lấy EmailTemplate từ TemplateId
        public async Task<EmailTemplate?> GetTemplateByIdAsync(string templateId)
        {
            return await _context.EmailTemplates
                .FirstOrDefaultAsync(t => t.TemplateId == templateId);
        }

        //Hàm send email
        public async Task<(bool Success, string ErrorMessage)> SendEmailAsync(
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
        )
        {
            try
            {
                // Lấy thông tin template nếu có
                if (!string.IsNullOrEmpty(templateId))
                {
                    var template = await GetTemplateByIdAsync(templateId);
                    if (template == null)
                    {
                        return (false, "Template not found.");
                    }

                    // Gộp các email mặc định từ template với email truyền vào
                    toEmails = CombineEmails(toEmails, template.DefaultToEmails);
                    ccEmails = CombineEmails(ccEmails, template.DefaultCcEmails);
                    bccEmails = CombineEmails(bccEmails, template.DefaultBccEmails);
                }
                
                 // Lấy danh sách email test nếu isTest là true
                List<string> emailTestList = new List<string>();

                if (isTest == true)
                {
                    emailTestList = await _context.EmailTestLists.Select(e => e.Email).ToListAsync();
                }

                // Lọc danh sách email dựa trên email test list
                List<string> validToEmails = ParseEmails(toEmails);
                List<string> validCcEmails = ParseEmails(ccEmails ?? "");
                List<string> validBccEmails = ParseEmails(bccEmails ?? "");

                if (isTest == true)
                {
                    validToEmails = validToEmails.Where(email => emailTestList.Contains(email)).ToList();
                    validCcEmails = validCcEmails.Where(email => emailTestList.Contains(email)).ToList();
                    validBccEmails = validBccEmails.Where(email => emailTestList.Contains(email)).ToList();
                }

                // Kiểm tra nếu không còn email nào để gửi sau khi lọc
                if (!validToEmails.Any())
                {
                    return (false, "No valid emails to send.");
                }

                //cấu hình smtp
                smtpAddress ??= _configuration["Smtp:Host"];
                portNumber ??= int.TryParse(_configuration["Smtp:Port"], out var port) ? port : 587;
                emailFrom ??= _configuration["Smtp:FromEmail"];
                password ??= _configuration["Smtp:Password"];

                string emailBody;

                if (!string.IsNullOrEmpty(body))
                {
                    emailBody = body;
                }
                else if (!string.IsNullOrEmpty(templateId))
                {
                    var template = await GetTemplateByIdAsync(templateId);
                    if (template == null)
                    {
                        return (false, "Template not found.");
                    }

                    emailBody = ApplyTemplate(template.Template, replacements ?? new Dictionary<string, string>());
                }
                else
                {
                    return (false, "No content available to send.");
                }


                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.Subject = subject;
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;

                    foreach (var toEmail in validToEmails)
                    {
                        mail.To.Add(toEmail);
                    }

                    mail.Priority = priority switch
                    {
                        "High" => MailPriority.High,
                        "Low" => MailPriority.Low,
                        _ => MailPriority.Normal,
                    };

                    
                    // Thêm email vào CC nếu thoả mãn điều kiện
                    if (validCcEmails.Any())
                    {
                        foreach (var ccEmail in validCcEmails)
                        {
                            mail.CC.Add(ccEmail);
                        }
                    }


                    // Thêm email vào BCC nếu thoả mãn điều kiện
                    if (validBccEmails.Any())
                    {
                        foreach (var bccEmail in validBccEmails)
                        {
                            mail.Bcc.Add(bccEmail);
                        }
                    }
                    

                    if (attachments != null)
                    {

                        foreach (var attachmentInfo in attachments)
                        {
                            var fileBytes = Convert.FromBase64String(attachmentInfo.Base64);
                            var stream = new MemoryStream(fileBytes);
                            var attachment = new Attachment(stream, attachmentInfo.FileName);
                            mail.Attachments.Add(attachment);
                            stream.Position = 0;
                        }
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber.Value))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                }
                }

                // Gọi hàm TrackEmailAsync để lưu vào EmailTracking, N
                await TrackEmailAsync(emailFrom, toEmails, subject, emailBody, templateId, ccEmails, bccEmails, priority, false, null, isTest, refId, dataSource, attachments != null && attachments.Any());

                return (true, string.Empty);
            }
            catch (SmtpException smtpEx)
            {
                await TrackEmailAsync(emailFrom, toEmails, subject, body ?? "", templateId, ccEmails, bccEmails, priority, true, smtpEx.Message, isTest, refId, dataSource, attachments != null && attachments.Any());
                return (false, $"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                await TrackEmailAsync(emailFrom, toEmails, subject, body ?? "", templateId, ccEmails, bccEmails, priority, true, ex.Message, isTest, refId, dataSource, attachments != null && attachments.Any());
                return (false, $"General Error: {ex.Message}");
            }
        }
    }
}
