using Microsoft.AspNetCore.Http;
namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadCoverAsync(IFormFile file);

        Task<string> UploadEBookAsync(IFormFile file);

        bool DeleteFile(string filePath);
    }
}