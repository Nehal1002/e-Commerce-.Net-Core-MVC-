using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.PayPalHelper;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicMenuProject.Controllers
{
    public class PaypalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaypalController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        ////[HttpPost]
        //public JsonResult UpdateTransaction(UpdateTransactionViewModel model)
        //{
        //    return Json("Saved successfully!");
        //}
        public IActionResult Transactions(string transactionId, string transactionStatus)
        {
            //string CaptureId = transactionId;
            //PayPalRefund.CapturesRefund(CaptureId, true).Wait();
            Transactions details = new Transactions();
            details.TransactionId = transactionId;
            details.TransactionStatus = transactionStatus;
            _context.Transactions.Add(details);
            _context.SaveChanges();
            return Json("Saved Successfully!");
        }
    }  

}