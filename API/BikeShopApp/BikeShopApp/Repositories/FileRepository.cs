using BikeShopApp.Interfaces;

namespace BikeShopApp.Repositories
{
    public class FileRepository : IFileRepository
    {
        public bool DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                return true;
            }
            else 
            {
                return false;
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string oldFilePath)
        {
            //Delete old Image.
            DeleteFile(oldFilePath);

            //Save new image, and return its file path.
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
