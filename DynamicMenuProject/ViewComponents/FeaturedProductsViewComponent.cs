using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.ViewComponents
{
    public class FeaturedProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public FeaturedProductsViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            //List<AddProductViewModel> model = new List<AddProductViewModel>();
            //model = (from product in _context.ProductNew
            //                  //join Permissions in _context.MenuPermissions.Where(r => r.RoleId == RoleId)
            //                  //on Menus.Id equals Permissions.MenuId
            //              select new AddProductViewModel
            //              {
            //                  ProductId = product.ProductId,
            //                  ProductName=product.ProductName,
            //                  Price = product.Price,
            //                  Image = product.Image,
            //              }).ToList();

            ////return View(model);
            ViewBag.product = _context.ProductNew.ToList();
            return View();
        }
    }
}
