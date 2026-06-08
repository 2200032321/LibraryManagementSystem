//using LibraryManagementSystem.BLL.Services.Interfaces;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//namespace LibraryManagementSystem.BLL.Services
//{
//    public class FileService : IFileService
//    {
//        private readonly IWebHostEnvironment _env;

//        public FileService(IWebHostEnvironment env)
//        {
//            _env = env;
//        }

//        public async Task<string> UploadCoverAsync(IFormFile file)
//        {
//            var folder =
//                Path.Combine(_env.WebRootPath, "Uploads", "Covers");

//            if (!Directory.Exists(folder))
//                Directory.CreateDirectory(folder);

//            var fileName =
//                Guid.NewGuid() + Path.GetExtension(file.FileName);

//            var path = Path.Combine(folder, fileName);

//            using var stream = new FileStream(path, FileMode.Create);

//            await file.CopyToAsync(stream);

//            return $"/Uploads/Covers/{fileName}";
//        }

//        public async Task<string> UploadEBookAsync(IFormFile file)
//        {
//            var folder =
//                Path.Combine(_env.WebRootPath, "Uploads", "EBooks");

//            if (!Directory.Exists(folder))
//                Directory.CreateDirectory(folder);

//            var fileName =
//                Guid.NewGuid() + Path.GetExtension(file.FileName);

//            var path = Path.Combine(folder, fileName);

//            using var stream = new FileStream(path, FileMode.Create);

//            await file.CopyToAsync(stream);

//            return $"/Uploads/EBooks/{fileName}";
//        }

//        public bool DeleteFile(string filePath)
//        {
//            var fullPath =
//                Path.Combine(_env.WebRootPath,
//                    filePath.TrimStart('/'));

//            if (!File.Exists(fullPath))
//                return false;

//            File.Delete(fullPath);

//            return true;
//        }

//        //public Task<string> UploadCoverAsync(IFormFile file)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public Task<string> UploadEBookAsync(IFormFile file)
//        //{
//        //    throw new NotImplementedException();
//        //}
//    }
//}