using AzureBlobFileUploader.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobFileUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _fileService.ListAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _fileService.UploadAsync(file);
            var blob = result.Blob;
            return Ok(new { url = blob.Uri });
        }

        [HttpGet]
        [Route("filename")]
        public async Task<IActionResult> Download(string filename)
        {
            var result = await _fileService.DownloadAsync(filename);
            if (result != null && result.Content != null && result.ContentType != null)
            {
                return File(result.Content, result.ContentType, result.Name);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("filename")]
        public async Task<IActionResult> Delete(string filename)
        {
            var result = await _fileService.DeleteAsync(filename);
            return Ok(result);
        }
    }
}
