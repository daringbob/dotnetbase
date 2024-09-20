using Microsoft.AspNetCore.Mvc;
using src.Dtos.Email;
using src.Repositories.Email;
using Microsoft.Extensions.Configuration;

namespace src.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        public EmailController(IEmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (request == null || (string.IsNullOrEmpty(request.TemplateId) && string.IsNullOrEmpty(request.Body)))
            {
                return BadRequest("Invalid request.");
            }
            int smtpPort = request.PortNumber ?? _configuration.GetValue<int>("Smtp:Port");

            var (success, errorMessage) = await _emailRepository.SendEmailAsync(
                request.ToEmails,
                request.Subject,
                request.Body,
                request.TemplateId,
                request.Replacements,
                request.Attachments,
                request.CcEmails,
                request.BccEmails,
                request.Priority,
                request.SmtpAddress,
                smtpPort,
                request.EmailFrom,
                request.Password,
                request.IsTest,
                request.RefId,
                request.DataSource
            );

            if (success)
            {
                return Ok("Email sent successfully!");
            }
            else
            {
                return BadRequest($"Failed to send email. Error: {errorMessage}");
            }
        }
    }
}
