using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Dx29.Services;

namespace Dx29.FileStorage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileStorage2Controller : Controller
    {
        const int SIZE_LIMIT = 2000 * 1024 * 1024; // 2 Gb

        public FileStorage2Controller(FileStorageService2 fileStorageService)
        {
            FileStorageService = fileStorageService;
        }

        public FileStorageService2 FileStorageService { get; }

        [RequestSizeLimit(SIZE_LIMIT)]
        [HttpGet("{userId}/{caseId}")]
        public async Task<IActionResult> DownloadAsync(string userId, string caseId, string path)
        {
            try
            {
                var stream = await FileStorageService.DownloadFileAsync(userId, caseId, path);
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

        [HttpGet("{userId}/{caseId}/share")]
        public IActionResult GetFileShare(string userId, string caseId, string path)
        {
            try
            {
                var share = FileStorageService.CreateFileShare(userId, caseId, path);
                return Ok(share);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RequestSizeLimit(SIZE_LIMIT)]
        [HttpPost("{userId}/{caseId}")]
        public async Task<IActionResult> UploadAsync(string userId, string caseId, string path)
        {
            if (Request.ContentLength > SIZE_LIMIT) return BadRequest("File size is too large.");

            try
            {
                var stream = Request.BodyReader.AsStream();
                await FileStorageService.UploadFileAsync(userId, caseId, path, stream);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> MoveOrCopyAsync(string userId, string source, string target, bool copy = false)
        {
            try
            {
                if (copy)
                {
                    await FileStorageService.CopyFileAsync(userId, source, target);
                }
                else
                {
                    await FileStorageService.MoveFileAsync(userId, source, target);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{sourceUserId}/{targetUserId}")]
        public async Task<IActionResult> MoveOrCopyAsync(string sourceUserId, string targetUserId, string source, string target, bool copy = false)
        {
            try
            {
                if (copy)
                {
                    await FileStorageService.CopyFileAsync(sourceUserId, targetUserId, source, target);
                }
                else
                {
                    await FileStorageService.MoveFileAsync(sourceUserId, targetUserId, source, target);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}/{caseId}")]
        public async Task<IActionResult> DeleteAsync(string userId, string caseId, string path)
        {
            try
            {
                await FileStorageService.DeleteFileAsync(userId, caseId, path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
