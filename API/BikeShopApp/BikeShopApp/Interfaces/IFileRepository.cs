using System.Net;

namespace BikeShopApp.Interfaces
{
    public interface IFileRepository
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UpdateFileAsync(IFormFile file, string oldFilePath);
        bool DeleteFile(string filePath);
    }
}
