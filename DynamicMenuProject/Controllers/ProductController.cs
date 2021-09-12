﻿using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProductController(ApplicationDbContext context, IHostingEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult GetProducts()
        {
            var user = _userManager.GetUserId(HttpContext.User);
            //var role = _userManager.GetUsersInRoleAsync(HttpContext);
            List<AddProductViewModel> lstmodel = new List<AddProductViewModel>();
            var models = _context.ProductNew.Where(m => m.UserId == user);//s.Where(m=>m.MenuGrpId!=0).OrderBy(m=>m.MenuGrpId);

            foreach (var item in models)
            {
                AddProductViewModel mvm = new AddProductViewModel();
                mvm.ProductId = item.ProductId;
                mvm.ProductName = item.ProductName;
                mvm.Price = item.Price;
                mvm.Description = item.Description;
                mvm.isActive = item.isActive;
                mvm.isDeleted = item.isDeleted;
                mvm.CreatedAt = item.CreatedAt;
                mvm.ModifiedAt = item.ModifiedAt;
                mvm.Image = item.Image;
                mvm.SubCategoryId = item.SubCategoryId;
                mvm.UserId = item.UserId;
                lstmodel.Add(mvm);
            }

            ViewBag.successMessage = TempData["successMessage"];
            return View(lstmodel);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductViewModel model)
        {
            if (model != null)
            {
                var productModel = new ProductNew();
                productModel.ProductName = model.ProductName;
                productModel.Price = model.Price;
                productModel.Description = model.Description;
                productModel.isActive = model.isActive;
                productModel.isDeleted = model.isDeleted;
                productModel.CreatedAt = DateTime.Now;
                productModel.ModifiedAt = DateTime.Now;
                productModel.SubCategoryId = model.SubCategoryId;
                productModel.UserId = _userManager.GetUserId(HttpContext.User);
                if (model.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    productModel.Image = DateTime.Now.ToString("yymmssfff") + extension;


                    string path = Path.Combine(wwwRootPath, "Images", productModel.Image);
                    var fileStream = new FileStream(path, FileMode.Create);
                    model.ImageFile.CopyTo(fileStream);
                }
                _context.ProductNew.Add(productModel);
                _context.SaveChanges();

                //return RedirectToAction("GetMenus");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditProduct(int ProductId)
        {
            AddProductViewModel mvm = new AddProductViewModel();
            var product = _context.ProductNew.FirstOrDefault(m => m.ProductId == ProductId);

            if (product != null)
            {
                mvm.ProductId = product.ProductId;
                mvm.ProductName = product.ProductName;
                mvm.Price = product.Price;
                mvm.Description = product.Description;
                mvm.isActive = product.isActive;
                mvm.isDeleted = product.isDeleted;
                mvm.CreatedAt = product.CreatedAt;
                mvm.ModifiedAt = product.ModifiedAt;
                mvm.Image = product.Image;
                mvm.SubCategoryId = product.SubCategoryId;
            }
            return View(mvm);
        }

        [HttpPost]
        public IActionResult EditProduct(AddProductViewModel model)
        {
            if (model != null)
            {
                var product = _context.ProductNew.FirstOrDefault(m => m.ProductId == model.ProductId);
                if (product != null)
                {
                    product.ProductName = model.ProductName;
                    product.Price = model.Price;
                    product.Description = model.Description;
                    product.isActive = model.isActive;
                    product.isDeleted = model.isDeleted;
                    //product.CreatedAt = model.CreatedAt;
                    product.ModifiedAt = DateTime.Now;
                    product.SubCategoryId = model.SubCategoryId;
                    var Image = model.Image;

                    if (model.ImageFile != null)
                    {
                        if (Image != model.Image)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                            string extension = Path.GetExtension(model.ImageFile.FileName);
                            product.Image = DateTime.Now.ToString("yymmssfff") + extension;


                            string path = Path.Combine(wwwRootPath, "Images", product.Image);
                            var fileStream = new FileStream(path, FileMode.Create);
                            model.ImageFile.CopyTo(fileStream);
                        }
                        else if (Image == null)
                        {
                            string wwwRootPath = _hostEnvironment.WebRootPath;
                            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                            string extension = Path.GetExtension(model.ImageFile.FileName);
                            product.Image = DateTime.Now.ToString("yymmssfff") + extension;


                            string path = Path.Combine(wwwRootPath, "Images", product.Image);
                            var fileStream = new FileStream(path, FileMode.Create);
                            model.ImageFile.CopyTo(fileStream);
                        }
                    }
                    _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    return RedirectToAction("GetProducts");
                }
                return View();
            }
            return View();
        }

        public IActionResult DeleteProduct(int Id)
        {
            var product = _context.ProductNew.FirstOrDefault(m => m.ProductId == Id);
            _context.ProductNew.Remove(product);
            _context.SaveChanges();

            TempData["successMessage"] = "Product Deleted Successfully.";
            TempData.Keep();

            return RedirectToAction("GetProducts");

        }

        [AllowAnonymous]
        public JsonResult Category()
        {
            var category = _context.Categories.Where(x => x.ParentId == 0).
                Select(x => new { value = x.Id, text = x.CategoryName }).ToList();
            return Json(category);
        }


        [AllowAnonymous]
        public JsonResult GetSubCategoryIdByCategoryId(int CategoryId)
        {
            var subCategory = _context.Categories.Where(x => x.ParentId == CategoryId).
                Select(x => new { value = x.Id, text = x.CategoryName }).ToList();
            return Json(subCategory);
        }

        public IActionResult ManageProductOrders()
        {
            List<RequestedProductsViewModel> listOrder = new List<RequestedProductsViewModel>();
            var userId = _userManager.GetUserId(HttpContext.User);
            var od = _context.OrderDetailsNew.ToList();
            if (userId != null)
            {
                var products = (from a in _context.OrderDetailsNew
                                join b in _context.ProductNew on a.ProductId equals b.ProductId
                                join o in _context.OrderNew on a.OrderId equals o.OrderId
                                select new RequestedProductsViewModel
                                {
                                    ProductId = b.ProductId,
                                    ProductName = b.ProductName,
                                    Image = b.Image
                                });
                RequestedProductsViewModel r = new RequestedProductsViewModel();
                List<OrderDtlViewModel> odx = new List<OrderDtlViewModel>();
                var orderDetail = (from a in _context.OrderDetailsNew
                                   join b in _context.ProductNew on a.ProductId equals b.ProductId
                                   join o in _context.OrderNew on a.OrderId equals o.OrderId
                                   select new OrderDtlViewModel
                                   {
                                       CustomerId =o.CustomerId,
                                       //CustomerName = b.ProductName,
                                       Price = a.Price,
                                       Quantity=a.Quantity,
                                       OrderId=o.OrderId,
                                       OrderStatus=o.OrderStatus,
                                       OrderDate=o.OrderDate
                                   });
                
                r.orderDtl = odx;
                listOrder.Add(r);
            }
            return View(listOrder);
        }

    }


}