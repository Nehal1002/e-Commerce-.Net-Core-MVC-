using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.PayPalHelper;
using DynamicMenuProject.View_Models;
using FinalProjectSecond.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DynamicMenuProject.Controllers
{
    public class CartNewController : Controller
    {
        private IConfiguration configuration { get; }
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartNewController(IConfiguration _configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            configuration = _configuration;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult CartNew()
        {
            List<CartNewViewModel> lstmodel = new List<CartNewViewModel>();
            var userId = _userManager.GetUserId(HttpContext.User);
            //var products = _context.ProductNew.Where(p => p.UserId == userId);
            var cartItems = _context.CartNew;
            var productsInCart=(from c in _context.CartNew 
                          join p in _context.ProductNew
                          on c.ProductId equals p.ProductId
                          where c.CustomerId == userId
                          select new CartNewViewModel
                          {
                              CartId = c.CartId,
                              CustomerId=c.CustomerId,
                              ProductId=p.ProductId,
                              ProductName=p.ProductName,
                              Price=p.Price,
                              Quantity=c.Quantity,
                              Image=p.Image,
                              TotalAmount=p.Price*c.Quantity
                          }).ToList();
            ViewBag.cartTotal= _context.CartNew.Where(c=>c.CustomerId==userId).Select(t=>t.TotalAmount).Sum();
            //foreach (var item in cartItems)
            //{
            //    CartNewViewModel cvm = new CartNewViewModel();
            //    cvm.CartId = item.CartId;
            //    cvm.CustomerId = item.CustomerId;
            //    cvm.ProductId = item.ProductId;
            //    cvm.Quantity = item.Quantity;
            //    cvm.TotalAmount = item.TotalAmount;
            //    lstmodel.Add(cvm);
            //}
            return View(productsInCart);

        }

        [AllowAnonymous]
        public JsonResult AddToCart(int productId)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Customer"))
            {
                var cart = new CartNew();
                //var cartProductId = _context.CartNew.First().ProductId;
                var cId= _userManager.GetUserId(HttpContext.User);
                var products = _context.CartNew.Where(c=>c.CustomerId==cId).Select(c => c.ProductId).ToList();
               
                var pId = _context.ProductNew.Where(p => p.ProductId == productId).FirstOrDefault().ProductId;
                foreach(var item in products)
                {
                    if(item==pId)
                    {
                        return Json("Product already exist in cart.");
                    }
                }
                
                    var product = _context.ProductNew.Where(p => p.ProductId == productId);
                    cart.ProductId = product.FirstOrDefault().ProductId;
                    cart.CustomerId = _userManager.GetUserId(HttpContext.User);
                    cart.Quantity = 1;
                    cart.TotalAmount = product.FirstOrDefault().Price;
                    _context.CartNew.Add(cart);
                    _context.SaveChanges();
                    return Json("Item added to cart. Please check cart.");
            }
            else
            {
                RedirectToAction("/Identity/Account/Login");
                return Json("Please login..");

            }
        }

        public JsonResult AddQuantity(int Quantity,int productId)
        {
           var mnp = _context.CartNew.FirstOrDefault(c => c.ProductId == productId);
           mnp.Quantity = Quantity + 1;
           mnp.TotalAmount = mnp.Quantity * mnp.TotalAmount;
            _context.Entry(mnp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

           _context.SaveChanges();
            RedirectToAction("CartNew");
            return Json("");
        }

        

        public IActionResult AddQuantityNew(int Quantity, int productId)
        {
            var cId = _userManager.GetUserId(HttpContext.User);
            var price = _context.ProductNew.Where(p => p.ProductId == productId).FirstOrDefault().Price;
            var mnp = _context.CartNew.Where(c=>c.CustomerId==cId).FirstOrDefault(c => c.ProductId == productId);
            mnp.Quantity = Quantity + 1;
            mnp.TotalAmount = mnp.Quantity * price;
            _context.Entry(mnp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();
            return RedirectToAction("CartNew");
        }

        public IActionResult SubtractQuantity(int Quantity, int productId)
        {
            var cId = _userManager.GetUserId(HttpContext.User);
            var price = _context.ProductNew.Where(p => p.ProductId == productId).FirstOrDefault().Price;
            if (Quantity > 1)
            {
                var mnp = _context.CartNew.Where(c => c.CustomerId == cId).FirstOrDefault(c => c.ProductId == productId);
                mnp.Quantity = Quantity - 1;
                mnp.TotalAmount = mnp.Quantity * price;
                _context.Entry(mnp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.SaveChanges();
            }
            return RedirectToAction("CartNew");
        }

        //public IActionResult AddToCart(int productId)
        //{
        //    if (User.Identity.IsAuthenticated && User.IsInRole("Customer"))
        //    {
        //        var cart = new CartNew();
        //        cart.CustomerId = _userManager.GetUserId(HttpContext.User);
        //        var product = _context.ProductNew.Where(p => p.ProductId == productId);
        //        cart.ProductId = product.FirstOrDefault().ProductId;
        //        cart.Quantity = 1;
        //        cart.TotalAmount = product.FirstOrDefault().Price;
        //        _context.CartNew.Add(cart);
        //        _context.SaveChanges();
        //        return RedirectToAction("CartNew");
        //    }
        //    else
        //    {
        //        return RedirectToAction("/Identity/Account/Login");
        //    }
        //}





        public IActionResult RemoveFromCart(int productId)
        {
            var cId = _userManager.GetUserId(HttpContext.User);
            var cartItem = _context.CartNew.Where(c => c.CustomerId == cId).FirstOrDefault(c => c.ProductId == productId);
            _context.CartNew.Remove(cartItem);
            _context.SaveChanges();
            
            return RedirectToAction("CartNew");
            //return Json("Product succcessfully removed from cart.");
        }
        
        public async Task<IActionResult> CreateOrder()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var productsInCart = (from c in _context.CartNew
                                  join p in _context.ProductNew
                                  on c.ProductId equals p.ProductId
                                  where c.CustomerId == userId
                                  select new CartNewViewModel
                                  {
                                      CartId = c.CartId,
                                      CustomerId = c.CustomerId,
                                      ProductId = p.ProductId,
                                      ProductName = p.ProductName,
                                      Price = p.Price,
                                      Quantity = c.Quantity,
                                      Image = p.Image,
                                      TotalAmount = p.Price * c.Quantity
                                  }).ToList();
            if (productsInCart.Count() <= 0)
            {
                return RedirectToAction("CartNew");
            }
            else
            {
                OrderNew orderNew = new OrderNew();
                orderNew.CustomerId = productsInCart.FirstOrDefault().CustomerId;
                orderNew.OrderDate = DateTime.Now;
                orderNew.TotalAmount = _context.CartNew.Where(c => c.CustomerId == userId).Select(t => t.TotalAmount).Sum();
                orderNew.OrderStatus = "Pending";
                Guid g = Guid.NewGuid();
                orderNew.UniqueId = g;

                _context.OrderNew.Add(orderNew);
                _context.SaveChanges();

                foreach (var item in productsInCart)
                {
                    OrderDetailsNew orderDetailsNew = new OrderDetailsNew();
                    orderDetailsNew.ProductId = item.ProductId;
                    orderDetailsNew.Price = item.Price;
                    orderDetailsNew.Quantity = item.Quantity;
                    orderDetailsNew.OrderId = orderNew.OrderId;
                    _context.OrderDetailsNew.Add(orderDetailsNew);
                    _context.SaveChanges();
                }
                var total = _context.CartNew.Where(c => c.CustomerId == userId).Select(t => t.TotalAmount).Sum();
                var UniqueId = orderNew.UniqueId;

                var CartNew = _context.CartNew.Where(c => c.CustomerId == userId);
                _context.CartNew.RemoveRange(CartNew);
                CartNew = null; //reset
                _context.SaveChanges();


                var payPalAPI = new PayPalAPI(configuration);
                string url = await payPalAPI.getRedirectURLToPayPal(Convert.ToDouble(total), "USD", UniqueId);

                return Redirect(url);
            }
            
        }
        public async Task<IActionResult> SuccessNew([FromQuery(Name = "paymentId")] string paymentId, [FromQuery(Name = "PayerId")] string payerID)
        {

            var payPalAPI = new PayPalAPI(configuration);
            PayPalPaymentExecutedResponse result = await payPalAPI.executedPayment(paymentId, payerID);
            
            var address = $"{result.payer.payer_info.shipping_address.recipient_name} " +
                $"{result.payer.payer_info.shipping_address.line1} " +
                $"{result.payer.payer_info.shipping_address.city}" +
                $" {result.payer.payer_info.shipping_address.country_code} " +
                $"{result.payer.payer_info.shipping_address.postal_code}";

            Payment details = new Payment();
            details.TransactionId = result.id;
            details.UniqueOrderId = result.transactions.FirstOrDefault().custom;
            details.Intent = result.intent;
            details.CartId = result.cart;
            details.State = result.state;
            details.PaymentMethod = result.payer.payment_method;
            details.Amount = result.transactions.FirstOrDefault()?.amount.total;
            details.TransactionFee = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.transaction_fee.value;
            details.CreatedAt = result.create_time;
            details.SaleId = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
            _context.Payment.Add(details);
            _context.SaveChanges();

            PayerInfo payerInfo = new PayerInfo();
            payerInfo.PaymentId = details.Id;
            payerInfo.PayerId = result.payer.payer_info.payer_id;
            payerInfo.Email = result.payer.payer_info.email;
            payerInfo.FirstName = result.payer.payer_info.first_name;
            payerInfo.LastName = result.payer.payer_info.last_name;
            payerInfo.CountryCode = result.payer.payer_info.country_code;
            payerInfo.ShippingAddress = address;
            _context.PayerInfo.Add(payerInfo);

            _context.SaveChanges();



            OrderNew orderNew = new OrderNew();
            var order = _context.OrderNew.Where(z => z.UniqueId == details.UniqueOrderId).Select(z => z.OrderStatus).Single();
            if (order=="Pending")
            {
                _context.OrderNew.Where(z => z.UniqueId == details.UniqueOrderId).Single().OrderStatus = "Successful";
                _context.SaveChanges();
            }



            ViewBag.SaleId = result.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
            ViewBag.result = result;


            var email = _userManager.GetUserName(HttpContext.User);
            SendEmail(email,"Order Details","Your order is successfully placed. Order Details: ");
            return View("SuccessNew");

        }

        public bool SendEmail(string email, string subject, string htmlMessage)
        {
            string fromMail = "kanoongonehal@gmail.com";
            string fromPassword = "ghoyypkmiafvjnyc";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);
            return true;
        }



        //public IActionResult MyOrders()
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    var orderItems = (from od in _context.OrderDetailsNew
        //                      //join p in _context.ProductNew on od.ProductId equals p.ProductId
        //                      join o in _context.OrderNew on od.OrderId equals o.OrderId
        //                      where o.CustomerId == userId
        //                      select new OrderViewModel
        //                      {
        //                          OrderId = o.OrderId,
        //                          OrderDate = o.OrderDate,
        //                          OrderStatus = o.OrderStatus,
        //                          UniqueId = o.UniqueId,
        //                          TotalAmount = od.Price * od.Quantity
        //                      }).ToList();
        //    var orders = new List<OrderViewModel>();
        //    foreach(var product in orders)
        //    {
        //        ViewBag.products=
        //    }
        //    ViewBag.orderItems = (from od in _context.OrderDetailsNew
        //                          join p in _context.ProductNew on od.ProductId equals p.ProductId
        //                          join o in _context.OrderNew on od.OrderId equals o.OrderId
        //                          where od.OrderId == o.OrderId
        //                          //where o.CustomerId == userId
        //                          select new OrderDetailsViewModel
        //                          {
        //                              OrderId = od.OrderId,
        //                              ProductId = od.ProductId,
        //                              ProductName = p.ProductName,
        //                              Price = od.Price,
        //                              Quantity = od.Quantity
        //                          }).ToList();
        //    List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
        //    ViewBag.x=(from item in orderViewModel
        //              join od in _context.OrderDetailsNew on item.OrderId equals od.OrderId
        //              join p in _context.ProductNew on od.ProductId equals p.ProductId
        //              select new OrderDetailsViewModel
        //              {
        //                  OrderId = od.OrderId,
        //                  ProductId = od.ProductId,
        //                  ProductName = p.ProductName,
        //                  Price = od.Price,
        //                  Quantity = od.Quantity
        //              }).ToList();
        //    return View(orderItems);
        //}

        public IActionResult MyOrders()
        {
            
            List<OrderViewModel> listOrder = new List<OrderViewModel>();
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                var orders = _context.OrderNew.Where(x => x.CustomerId == userId).ToList();
                foreach (var order in orders)
                {
                    OrderViewModel odr = new OrderViewModel();
                    odr.OrderId = order.OrderId;
                    odr.CustomerId = order.CustomerId;
                    odr.OrderStatus = order.OrderStatus;
                    odr.UniqueId = order.UniqueId;
                    odr.OrderDate = order.OrderDate;
                    
                    var orderDetail = _context.OrderDetailsNew.Where(x => x.OrderId == odr.OrderId).ToList();
                    List<OrderDetailsViewModel> od = new List<OrderDetailsViewModel>();
                    foreach (var product in orderDetail)
                    {
                        OrderDetailsViewModel orderDlItem = new OrderDetailsViewModel();
                        orderDlItem.OrderId = product.OrderId;
                        orderDlItem.ProductId = product.ProductId;
                        orderDlItem.Image = _context.ProductNew.Where(c => c.ProductId == product.ProductId).FirstOrDefault().Image;
                        orderDlItem.ProductName = _context.ProductNew.Where(c => c.ProductId == product.ProductId).FirstOrDefault().ProductName;
                        orderDlItem.Price = product.Price;
                        orderDlItem.Quantity = product.Quantity;
                        od.Add(orderDlItem);
                    }
                    odr.orderDetailsViewModel = od;
                    listOrder.Add(odr);
                }
            }
            return View(listOrder);
        }

        public IActionResult AdminOrders()
        {
            List<OrderViewModel> listOrder = new List<OrderViewModel>();
            var orders = _context.OrderNew.ToList();
            foreach (var order in orders)
            {
                OrderViewModel odr = new OrderViewModel();
                odr.OrderId = order.OrderId;
                odr.CustomerId = order.CustomerId;
                odr.OrderStatus = order.OrderStatus;
                odr.UniqueId = order.UniqueId;
                odr.OrderDate = order.OrderDate;

                var orderDetail = _context.OrderDetailsNew.Where(x => x.OrderId == odr.OrderId).ToList();
                List<OrderDetailsViewModel> od = new List<OrderDetailsViewModel>();
                foreach (var product in orderDetail)
                {
                    OrderDetailsViewModel orderDlItem = new OrderDetailsViewModel();
                    orderDlItem.OrderId = product.OrderId;
                    orderDlItem.ProductId = product.ProductId;
                    orderDlItem.Image = _context.ProductNew.Where(c => c.ProductId == product.ProductId).FirstOrDefault().Image;
                    orderDlItem.ProductName = _context.ProductNew.Where(c => c.ProductId == product.ProductId).FirstOrDefault().ProductName;
                    orderDlItem.Price = product.Price;
                    orderDlItem.Quantity = product.Quantity;
                    od.Add(orderDlItem);
                }
                odr.orderDetailsViewModel = od;
                listOrder.Add(odr);
            }
            return View();
        }
    }
}