using System.Collections.Generic;
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

                var storeRecord = await _storeRepo.CreateFile(serverRelativeUrl, fileName, refID, dataSource, file);
                return Ok(storeRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/store/create-folder
        [HttpPost("create-folder")]
        public IActionResult CreateFolder(string serverRelativeUrl, int? refID, string? dataSource)
        {
            try
            {
                var storeRecord = _storeRepo.CreateFolder(serverRelativeUrl, refID, dataSource);
                return Ok(storeRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/store/get-file-content
        [HttpGet("get-file-content")]
        public IActionResult GetFileContent(string serverRelativeUrl)
        {
            try
            {
                var fileBytes = _storeRepo.GetFileContent(serverRelativeUrl);
                
                return File(fileBytes, "application/octet-stream");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        // GET: api/store/get-folder-content
        [HttpGet("get-folder-info")]
        public IActionResult GetFolderInfo(string serverRelativeUrl, int? refID, string? dataSource)
        {
            try
            {
                var filesInfo = _storeRepo.GetFolderInfo(serverRelativeUrl, refID, dataSource);
                return Ok(filesInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/store/delete-file
        [HttpDelete("delete-file")]
        public IActionResult DeleteFile(string serverRelativeUrl)
        {
            try
            {
                _storeRepo.DeleteFile(serverRelativeUrl);
                return Ok("File deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/store/delete-folder
        [HttpDelete("delete-folder")]
        public IActionResult DeleteFolder(string serverRelativeUrl)
        {
            try
            {
                _storeRepo.DeleteFolder(serverRelativeUrl);
                return Ok("Folder deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
