using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.Net.Http;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UploadDownloadFiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        // POST api/<FileController>/UploadFile
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var bytes = await System.IO.File.ReadAllBytesAsync(path);

            return File(bytes, "application/pdf", filename);
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> DownloadFile(string filename)
        //{
        //    if (filename == null)
        //        return Content("filename not present");

        //    var path = Path.Combine(
        //                   Directory.GetCurrentDirectory(),
        //                   "wwwroot", filename);

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
