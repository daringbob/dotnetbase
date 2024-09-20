using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Logs : ControllerBase
    {
        private readonly string _logDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "logs"
        );

        [HttpGet("{date}")]
        public IActionResult GetLogFileByDate(string date)
        {
            try
            {
                if (DateTime.TryParse(date, out var logDate))
                {
                    // Assuming log files are named like "stdout_yyyyMMdd_HHmmss.log"
                    var logFileName = Directory
                        .GetFiles(_logDirectory, $"stdout_{logDate:yyyyMMdd}*.log")
                        .FirstOrDefault();

                    if (logFileName != null && System.IO.File.Exists(logFileName))
                    {
                        var fileBytes = System.IO.File.ReadAllBytes(logFileName);
                        var fileName = Path.GetFileName(logFileName);

                        return File(fileBytes, "application/octet-stream", fileName);
                    }
                    else
                    {
                        return NotFound($"Log file for date {date} not found.");
                    }
                }
                else
                {
                    return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
