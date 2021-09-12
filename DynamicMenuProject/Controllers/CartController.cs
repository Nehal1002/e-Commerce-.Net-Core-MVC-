using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.PayPalHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Rotativa.AspNetCore;

namespace DynamicMenuProject.Controllers
{
    //[Route("cart")]
    public class CartController : Controller
    {
        private IConfiguration configuration { get; }

        private readonly ApplicationDbContext _context;

        public CartController(IConfiguration _configuration, ApplicationDbContext context)
        {
            configuration = _configuration;
            _context = context;
        }


        //[Route("index")]
        //[Route("")]
        //[Route("~/")]
        public IActionResult Index()
        {
            var productModel = new ProductModel();
            ViewBag.products = productModel.FindAll();
            ViewBag.total = productModel.FindAll().Sum(p => p.Price * p.Quantity);
            return View();
        }

        [HttpPost]
        //[Route("checkout")]
        public async Task<IActionResult> Checkout(double total)
        {
            Guid g = Guid.NewGuid();
            var payPalAPI = new PayPalAPI(configuration);
            string url = await payPalAPI.getRedirectURLToPayPal(total, "USD", g);

            return Redirect(url);
        }

        //[Route("success")]
        public async Task<IActionResult> Success([FromQuery(Name = "paymentId")] string paymentId, [FromQuery(Name = "PayerId")] string payerID)
        {

            var payPalAPI = new PayPalAPI(configuration);
            PayPalPaymentExecutedResponse result = await payPalAPI.executedPayment(paymentId, payerID);
            Debug.WriteLine("Transaction Details");
            Debug.WriteLine("cart: " + result.cart);
            Debug.WriteLine("create_time: " + result.create_time.ToLongDateString());
            Debug.WriteLine("id: " + result.id);
            Debug.WriteLine("intent: " + result.intent);
            Debug.WriteLine("link 0 - href: " + result.links[0].href);
            Debug.WriteLine("link 0 - method: " + result.links[0].method);
            Debug.WriteLine("link 0 - rel: " + result.links[0].rel);
            Debug.WriteLine("payer_info - first_name: " + result.payer.payer_info.first_name);
            Debug.WriteLine("payer_info - last_name: " + result.payer.payer_info.last_name);
            Debug.WriteLine("payer_info - email: " + result.payer.payer_info.email);
            Debug.WriteLine("payer_info - billing_address: " + result.payer.payer_info.billing_address);
            Debug.WriteLine("payer_info - country_code: " + result.payer.payer_info.country_code);
            Debug.WriteLine("payer_info - shipping_address: " + result.payer.payer_info.shipping_address);
            Debug.WriteLine("payer_info - payer_id: " + result.payer.payer_info.payer_id);
            Debug.WriteLine("state: " + result.state);
            Debug.WriteLine("Sale Id: " + result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id);
            var address = $"{result.payer.payer_info.shipping_address.recipient_name} {result.payer.payer_info.shipping_address.line1} {result.payer.payer_info.shipping_address.city} {result.payer.payer_info.shipping_address.country_code} {result.payer.payer_info.shipping_address.postal_code}";
            Cart details = new Cart();
            details.TransactionId = result.id;
            details.Intent = result.intent;
            details.CartId = result.cart;
            details.State = result.state;
            //lst.OrderId = result.id;
            details.PayerId = result.payer.payer_info.payer_id;
            details.Email = result.payer.payer_info.email;
            details.FirstName = result.payer.payer_info.first_name;
            details.LastName = result.payer.payer_info.last_name;
            details.CountryCode = result.payer.payer_info.country_code;
            details.PaymentMethod = result.payer.payment_method;
            details.Amount = result.transactions.FirstOrDefault()?.amount.total;
            details.ShippingAddress = address;
            details.TransactionFee = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.transaction_fee.value;
            details.CreateDate = result.create_time;
            details.SaleId = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
            _context.Cart.Add(details);
            _context.SaveChanges();

            ViewBag.SaleId = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
            ViewBag.result = result;
            return View("Success");
        }

        public IActionResult Invoice(string Id)
        {
            var inc = _context.Cart.Where(x => x.TransactionId == Id).FirstOrDefault();
            return View(inc);
        }
        public IActionResult InvoicePdf(string Id)
        {
            var inc = _context.Cart.Where(x => x.TransactionId == Id).FirstOrDefault();
            return new ViewAsPdf("InvoicePdf", inc);
        }
        public async Task<IActionResult> Refund(string Id)
        { 
            string CaptureId = Id;
            PayPalRefund.CapturesRefund(CaptureId, true).Wait();
            return View();
        }
    }
}