using Microsoft.AspNetCore.Http;

namespace BikeShopApp.Core.RepositoryInterfaces
{
    public interface IFileRepository
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UpdateFileAsync(IFormFile file, string oldFilePath);
        void DeleteFile(string filePath);
    }
}
