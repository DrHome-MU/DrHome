
namespace Dr_Home.File_Manager
{
    public class FileManager(IWebHostEnvironment _webHostEnvironment) : IFileManager
    {
        private readonly string _filePath  = $"{_webHostEnvironment.WebRootPath}/Pictures";

        

        public async Task<string> Upload(IFormFile file, CancellationToken cancellationToken)
        {
            var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";


            var path = Path.Combine(_filePath, randomFileName);

            using var stream = File.Create(path);

            await file.CopyToAsync(stream, cancellationToken);

            return "http://dr-home.runasp.net/Pictures/" + randomFileName;
        }

        public async Task<bool> Delete(string fileUrl)
        {
            try
            {

                string fileName = Path.GetFileName(new Uri(fileUrl).AbsolutePath);


                string fullPath = Path.Combine(_filePath, fileName);


                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting file: " + ex.Message);
            }
            
            return false;
        }
    }
}
