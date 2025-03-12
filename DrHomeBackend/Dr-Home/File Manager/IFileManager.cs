namespace Dr_Home.File_Manager
{
    public interface IFileManager
    {
        Task<string>Upload(IFormFile file , CancellationToken cancellationToken);

        Task<bool> Delete(string fileUrl);

    }
}
