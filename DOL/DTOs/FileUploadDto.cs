using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.DOL.DTOs
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }
}