using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string oldFilePath)
        {
            //Delete old image.
            DeleteFile(oldFilePath);

            //Save new image and return its file path.
            return await UploadFileAsync(file);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string folderName = Path.Combine("Resources", "Images");

            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string dbPath = Path.Combine(folderName, fileName);

            string fullPath = Path.Combine(pathToSave, fileName);


            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return dbPath;
        }
    }
}
