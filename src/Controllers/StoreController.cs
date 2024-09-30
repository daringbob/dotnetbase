using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Repositories.Store;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepo;

        public StoreController(IStoreRepository storeRepo)
        {
            _storeRepo = storeRepo;
        }

        private bool CheckServerRelativeUrlValidity(string serverRelativeUrl)
        {
            if (!serverRelativeUrl.StartsWith('/') || (serverRelativeUrl.EndsWith('/') && serverRelativeUrl.Length > 1))
            {
                return false;
            }

            return true;
        }

        // POST: api/store/create-file
        [HttpPost("create-file")]
        public async Task<IActionResult> CreateFile(string serverRelativeUrl, string fileName, int? refID, string? dataSource, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                if (!CheckServerRelativeUrlValidity(serverRelativeUrl) || fileName.Contains('/'))
                {
                    return BadRequest("Invalid serverRelativeUrl.");
                }
                var storeRecord = await _storeRepo.CreateFile(serverRelativeUrl, fileName, refID, dataSource, file.OpenReadStream());
                return Ok(storeRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/store/get-file-info
        [HttpGet("get-file-info")]
        public IActionResult GetFileInfo(string serverRelativeUrl)
        {
            try
            {
                if (!CheckServerRelativeUrlValidity(serverRelativeUrl))
                {
                    return BadRequest("Invalid serverRelativeUrl.");
                }
                var fileInfo = _storeRepo.GetFileInfo(serverRelativeUrl);
                if (fileInfo == null)
                {
                    return NotFound("File not found.");
                }
                return Ok(fileInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/store/delete-file
        [HttpDelete("delete-file")]
        public async Task<IActionResult> DeleteFile(string serverRelativeUrl)
        {
            try
            {
                if (!CheckServerRelativeUrlValidity(serverRelativeUrl))
                {
                    return BadRequest("Invalid serverRelativeUrl.");
                }
                await _storeRepo.DeleteFile(serverRelativeUrl);
                return Ok("File deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
