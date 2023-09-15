using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.WebAPI.Controllers
{
    public class FileController : CustomControllerBase
    {
        private readonly IFileRepository _fileRepository;

        /// <summary>
        /// File Controller Constructor.
        /// </summary>
        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }


        /// <summary>
        /// Upload the file that was passed in the form data.
        /// </summary>
        [HttpPost, RequestSizeLimit(100_000_000)]
        [AllowAnonymous]
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
                return Problem(detail: "No files were passed.", statusCode: 400, title: "Bad Request");
            }

            return Ok(new { dbPath });
        }

        /// <summary>
        /// Delete the file at the passed filePath and upload the file in the form data.
        /// </summary>
        /// <param name="oldFilePath"></param>
        [HttpPut, RequestSizeLimit(100_000_000)]
        [AllowAnonymous]
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
                return Problem(detail: "No files were passed.", statusCode: 400, title: "Bad Request");
            }

            return Ok(new { dbPath });
        }

        /// <summary>
        /// Delete the file at the passed filePath.
        /// </summary>
        /// <param name="filePath"></param>
        [HttpDelete]
        [AllowAnonymous]
        public IActionResult DeleteFile([FromQuery] string filePath)
        {
            if (filePath != "" && filePath != null)
            {
                _fileRepository.DeleteFile(filePath);

                return NoContent();
            }
            else
            {
                return Problem(detail: "No file path was passed.", statusCode: 400, title: "Bad Request");
            }
        }
    }
}
