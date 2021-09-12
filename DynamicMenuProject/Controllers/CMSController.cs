using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DynamicMenuProject.Controllers
{
    public class CMSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostEnvironment;

        //public object _hostEnvironment { get; private set; }

        public CMSController(ApplicationDbContext context,IHostingEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //string wwwPath = this._.WebRootPath;

            List<CMSItemsViewModel> lstmodel = new List<CMSItemsViewModel>();
            var models = _context.CMSItems;

            foreach (var item in models)
            {
                CMSItemsViewModel mvm = new CMSItemsViewModel();
                mvm.Id = item.Id;
                mvm.PageName = item.PageName;
                mvm.PageUrl = item.PageUrl;
                mvm.Description = item.Description;
                mvm.BannerImage = item.BannerImage;
                lstmodel.Add(mvm);
            }
            
            return View(lstmodel);
        }
        [HttpGet]
        public IActionResult CreatePage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePage(CMSItemsViewModel model)
        {
            if (model != null)
            {
                var cmsModel = new CMSItems();
                cmsModel.PageName = model.PageName;
                cmsModel.PageUrl = model.PageUrl;
                cmsModel.Description = model.Description;
                if (model.BannerImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);
                    string extension = Path.GetExtension(model.BannerImageFile.FileName);
                    cmsModel.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;


                    string path = Path.Combine(wwwRootPath, "Images", cmsModel.BannerImage);
                    //var mem = new MemoryStream();
                    //user.ProfilePicture
                    var fileStream = new FileStream(path, FileMode.Create);
                    model.BannerImageFile.CopyTo(fileStream);
                }
                //cmsModel.BannerImage = model.BannerImage;
                _context.CMSItems.Add(cmsModel);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditPage(int Id)
        {
            CMSItemsViewModel mvm = new CMSItemsViewModel();
            var page = _context.CMSItems.FirstOrDefault(m => m.Id == Id);

            if (page != null)
            {
                mvm.PageName = page.PageName;
                mvm.PageUrl = page.PageUrl;
                mvm.Description = page.Description;
                mvm.BannerImage = page.BannerImage;
            }
            return View(mvm);
        }

        [HttpPost]
        public IActionResult EditPage(CMSItemsViewModel model)
        {
            if (model != null)
            {
                var PageItem = _context.CMSItems.FirstOrDefault(m => m.Id == model.Id);
                if (PageItem != null)
                {
                    PageItem.PageName = model.PageName;
                    PageItem.PageUrl = model.PageUrl;
                    PageItem.Description = model.Description;
                    var BannerImage = PageItem.BannerImage;

                    if (model.BannerImageFile != null)
                    {
                        if (BannerImage != model.BannerImage)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);
                            string extension = Path.GetExtension(model.BannerImageFile.FileName);
                            PageItem.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;


                            string path = Path.Combine(wwwRootPath, "Images", PageItem.BannerImage);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);
                            model.BannerImageFile.CopyTo(fileStream);
                        }
                        else if(BannerImage==null)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);
                            string extension = Path.GetExtension(model.BannerImageFile.FileName);
                            PageItem.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;


                            string path = Path.Combine(wwwRootPath, "Images", PageItem.BannerImage);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);
                            model.BannerImageFile.CopyTo(fileStream);
                        }
                    }
                    _context.Entry(PageItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }

       // [HttpPost]
        public IActionResult DeletePage(int Id)
        {
            var page = _context.CMSItems.FirstOrDefault(m => m.Id == Id);
            _context.CMSItems.Remove(page);
            _context.SaveChanges();

            TempData["successMessage"] = "Page Deleted Successfully.";
            TempData.Keep();

            return RedirectToAction("Index");

        }
        public JsonResult IsPageNameExist(string pageName, int? Id)
        {
            var validateName = _context.CMSItems.FirstOrDefault(x => x.PageName == pageName && x.Id != Id );

            if (validateName != null)
            {
                return Json(false, new JsonSerializerSettings());
            }
            else
            {
                return Json(true, new JsonSerializerSettings());
            }
        }
        public JsonResult IsPageUrlExist(string pageUrl, int? Id)
        {
            var validateName = _context.CMSItems.FirstOrDefault(x => x.PageUrl == pageUrl && x.Id != Id);

            if (validateName != null)
            {
                return Json(false, new JsonSerializerSettings());
            }
            else
            {
                return Json(true, new JsonSerializerSettings());
            }
        }
    }
}