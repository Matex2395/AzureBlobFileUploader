using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobFileUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            return Ok("Hello World");
        }

        [HttpGet]
        [Route("filename")]
        public async Task<IActionResult> Download(string filename)
        {
            return Ok("Hello World");
        }
    }
}
