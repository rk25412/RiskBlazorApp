using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Clear.Risk.Infrastructures.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Clear.Risk.Controllers.Api
{

    [Route("api/upload")]
    public class UploadApiController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public UploadApiController(IWebHostEnvironment env)
        {
            _environment = env;
        }

        [HttpPost("UploadIconImage")]
        public IActionResult UploadIcon(IFormFile file)
        {
            try
            {
                UploadIconImage(file);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        public async Task UploadIconImage(IFormFile file)
        {
            if (file != null && file.Length != 0)
            {
                //var uploadPath = _environment.WebRootPath + @"\Uploads\Templates\PPEImageIcon\";
                var uploadPath = _environment.WebRootPath + @"\PPEImageIcon\";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fullpath = Path.Combine(uploadPath, file.FileName);
                using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        [HttpPost("single/{name}")]
        public IActionResult Single(IFormFile file, string name)
        {
            try
            {
                UploadFile(file, name);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        public async Task UploadFile(IFormFile file, string name)
        {
            if (file != null && file.Length != 0)
            {
                var uploadPath = _environment.WebRootPath + @"\UploadDocument\" + User.Identity.GetUserOrgId().ToString();
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var fullpath = Path.Combine(uploadPath, name + ext);
                using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        [HttpPost("uploadProfilePic")]
        public async Task<IActionResult> UploadProfilePic(IFormFile file)
        {
            try
            {
                if (file != null && file.Length != 0)
                {
                    var uploadPath = _environment.WebRootPath + @"\ProfilePictures\";
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fullpath = Path.Combine(uploadPath, file.FileName);
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("blog")]
        public async Task<IActionResult> Blog(IFormFile file)
        {
            try
            {
                if (file != null && file.Length != 0)
                {
                    var uploadPath = _environment.WebRootPath + @"\Blog\";
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fullpath = Path.Combine(uploadPath, file.FileName);
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("HowToAddPdf")]
        public async Task<IActionResult> HowToAddPdf(IFormFile file)
        {
            try
            {
                if (file != null && file.Length != 0)
                {
                    var uploadPath = _environment.WebRootPath + @"\Upload\HowTo\pdf";
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fullpath = Path.Combine(uploadPath, file.FileName);
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("HowToAddVideo")]
        public async Task<IActionResult> HowToAddVideo(IFormFile file)
        {
            try
            {
                if (file != null && file.Length != 0)
                {
                    var uploadPath = _environment.WebRootPath + @"\Upload\HowTo\Video";
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fullpath = Path.Combine(uploadPath, file.FileName);
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
