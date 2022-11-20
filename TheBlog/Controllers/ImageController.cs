using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBlog.Utilities;

namespace TheBlog.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly FileValidationService _fileValidationService;

        public ImageController(FileValidationService fileValidationService)
        {
            _fileValidationService = fileValidationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Image")]
        [RequestSizeLimit(5_242_880)] // Max file size 5Mb in bytes
        public async Task<IActionResult> FileUpload(IFormFile uploadFile)
        {
            if (ModelState.IsValid == false)
                return View("Index");

            // Validate file
            if (_fileValidationService.IsValid(uploadFile) == false)
            {
                ModelState.AddModelError("File", "File type is invalid. Only *.jpg or *.png are accepted file formats.");
                return View("Index");
            }

            // Create a unique file name
            var filePath = $"wwwroot/Images/{Guid.NewGuid()}.{uploadFile.FileName.Split('.').Last()}";

            // Save file to disk
            using (var stream = System.IO.File.Create(filePath))
            {
                await uploadFile.CopyToAsync(stream);
            }

            return View("Index");
        }
    }
}
