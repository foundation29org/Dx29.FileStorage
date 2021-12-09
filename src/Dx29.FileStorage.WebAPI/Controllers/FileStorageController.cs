using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Dx29.Services;

namespace Dx29.FileStorage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileStorageController : Controller
    {
        const int SIZE_LIMIT = 2000 * 1024 * 1024; // 2 Gb

        public FileStorageController(FileStorageService fileStorageService)
        {
            FileStorageService = fileStorageService;
        }

        public FileStorageService FileStorageService { get; }

        [RequestSizeLimit(SIZE_LIMIT)]
        [HttpPost("file/{userId}/{caseId}/{resourceId}/{name}")]
        public async Task<IActionResult> UploadAsync(string userId, string caseId, string resourceId, string name)
        {
            if (name.Length > 128) return BadRequest("Name too large.");
            if (Request.ContentLength > SIZE_LIMIT) return BadRequest("File size is too large.");

            try
            {
                var stream = Request.BodyReader.AsStream();
                await FileStorageService.UploadFileAsync(userId, caseId, resourceId, name, stream);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RequestSizeLimit(SIZE_LIMIT)]
        [HttpGet("file/{userId}/{caseId}/{resourceId}/{name}")]
        public async Task<IActionResult> DownloadAsync(string userId, string caseId, string resourceId, string name)
        {
            if (name.Length > 128) return BadRequest("Name too large.");

            try
            {
                var stream = await FileStorageService.DownloadFileAsync(userId, caseId, resourceId, name);
                if (stream != null)
                {
                    return File(stream, "application/octet-stream");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("file/{userId}/{caseId}/{resourceId}/{name}")]
        public async Task<IActionResult> DeleteAsync(string userId, string caseId, string resourceId, string name)
        {
            if (name.Length > 128) return BadRequest("Name too large.");

            try
            {
                await FileStorageService.DeleteFileAsync(userId, caseId, resourceId, name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("copy/{userId}/{source}/{target}/{resourceId}/{name}")]
        public async Task<IActionResult> CopyAsync(string userId, string source, string target, string resourceId, string name)
        {
            try
            {
                await FileStorageService.CopyFileAsync(userId, source, target, resourceId, name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("move/{userId}/{source}/{target}/{resourceId}/{name}")]
        public async Task<IActionResult> MoveAsync(string userId, string source, string target, string resourceId, string name)
        {
            try
            {
                await FileStorageService.MoveFileAsync(userId, source, target, resourceId, name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("share/{userId}/{caseId}/{resourceId}/{name}")]
        public IActionResult CreateFileShare(string userId, string caseId, string resourceId, string name)
        {
            try
            {
                var share = FileStorageService.CreateFileShare(userId, caseId, resourceId, name);
                return Ok(share);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
