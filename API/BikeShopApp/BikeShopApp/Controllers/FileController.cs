using BikeShopApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpPost, RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadFile() 
        {
            var file = Request.Form.Files[0];
            string dbPath;

            if (file.Length > 0)
            {
                dbPath = await _fileRepository.UploadFileAsync(file);
            }
            else 
            {
                return BadRequest(ModelState);
            }

            return Ok(new { dbPath });
        }


        [HttpPut, RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UpdateFile([FromQuery] string oldFilePath)
        {
            var file = Request.Form.Files[0];
            string dbPath;

            if (file.Length > 0)
            {
                dbPath = await _fileRepository.UpdateFileAsync(file, oldFilePath);
            }
            else
            {
                return BadRequest(ModelState);
            }

            return Ok(new { dbPath });
        }


        [HttpDelete]
        public IActionResult DeleteFile([FromQuery] string filePath)
        {
            if (filePath != "" && filePath != null)
            {
                if (_fileRepository.DeleteFile(filePath))
                {
                    return Ok();
                }
                else 
                {
                    return NotFound("No file found at that filePath");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
