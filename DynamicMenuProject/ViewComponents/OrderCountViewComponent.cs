using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.ViewComponents
{
    public class OrderCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderCountViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var cId = _userManager.GetUserId(HttpContext.User);
            ViewBag.count = _context.OrderNew.Where(c => c.CustomerId == cId).Count();
            return View();
        }
    }
}
