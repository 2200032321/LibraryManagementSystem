//using LibraryManagementSystem.BLL.Services.Interfaces;
//using LibraryManagementSystem.DOL.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;

//namespace LibraryManagementSystem.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class FilesController : ControllerBase
//    {
//        private readonly IFileService _service;

//        public FilesController(IFileService service)
//        {
//            _service = service;
//        }

//        [HttpPost("cover")]
//        [Authorize(Roles = "Admin,Librarian")]
//        public async Task<IActionResult> UploadCover(
//            IFormFile file)
//        {
//            var path =
//                await _service.UploadCoverAsync(file);

//            return Ok(
//                ApiResponse<string>.Ok(
//                    path,
//                    "Cover uploaded"));
//        }

//        [HttpPost("ebook")]
//        [Authorize(Roles = "Admin,Librarian")]
//        public async Task<IActionResult> UploadEBook(
//            IFormFile file)
//        {
//            var path =
//                await _service.UploadEBookAsync(file);

//            return Ok(
//                ApiResponse<string>.Ok(
//                    path,
//                    "EBook uploaded"));
//        }

//        [HttpDelete]
//        [Authorize(Roles = "Admin")]
//        public IActionResult Delete(string path)
//        {
//            var result =
//                _service.DeleteFile(path);

//            return result
//                ? Ok(ApiResponse<string>.Ok("", "Deleted"))
//                : NotFound();
//        }
//    }
//}