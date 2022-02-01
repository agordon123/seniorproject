using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanOpticon.Data;
using PanOpticon.Models;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PanOpticon.Controllers
{
    public class FileController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<PanopticonUser> _userManager;

        public FileController(IHostingEnvironment hosting, ApplicationDbContext application, UserManager<PanopticonUser> userManager)
        {
            hostingEnvironment = hosting;
            applicationDbContext = application;
            _userManager = userManager;

        }
        // GET: FileController

   

        [Authorize]
        public ActionResult Index()
        {
            var array = Directory.GetFiles(Path.Combine(hostingEnvironment.ContentRootPath, "files"));
            List<FileVM> files = new();
            var task = applicationDbContext.Files.Include(p => p.PanopticonUser).Where(u => u.PanopticonUser.Id == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToList();
            task.ForEach(x =>
            {
                array.ToList().ForEach(y =>
                {
                    if (x.FileName.Contains(y))
                    {
                        files.Add(new FileVM {Name=x.ImageName, Path = x.FileName });
                    }
                });
            });
           
            return View( files  ) ;
        }

        [Authorize]
        public ActionResult Download(string path, string name)
        {
            byte[] filebytes = System.IO.File.ReadAllBytes(path);
            return File(filebytes ,"application/force-download", name);
        }
        // GET: FileController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileController/Create
        [DisableRequestSizeLimit]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(FileUploadVM fileUploadVM)
        {
            if(ModelState.IsValid)
            {
                var f = new FileUpload();
                f.PanopticonUser = await _userManager.GetUserAsync(HttpContext.User);
                f.Guid = new Guid();
                f.ImageName = Path.GetFileName(fileUploadVM.File.FileName);
                f.FileName = Path.Combine(hostingEnvironment.ContentRootPath, "files", f.ImageName);
                applicationDbContext.Files.Add(f);
                await applicationDbContext.SaveChangesAsync();

                if (!Directory.Exists(Path.Combine(hostingEnvironment.ContentRootPath, "files")))
                {
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "files"));
                }

                using (var file = new FileStream(f.FileName, FileMode.Create))
                {
                   
                    await fileUploadVM.File.CopyToAsync(file);
                }

                RedirectToAction(nameof(Index));
            }
            return View(fileUploadVM);
        }

        // GET: FileController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: FileController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: FileController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: FileController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
