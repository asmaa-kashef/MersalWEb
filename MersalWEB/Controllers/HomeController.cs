using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using MersalWEB.Services;
using Microsoft.CodeAnalysis;
using MersalWEB.Helpers;
using MersalWEB.shoppingcart;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Hosting.Server;

namespace MersalWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender; 
        public object CartId { get; private set; }
        private readonly ApplicationDbContext db;
        List<Cart> shoppingcarts = new List<Cart>();
        public HomeController(ApplicationDbContext context ,IEmailSender emailSender)
        {
           _emailSender = emailSender;
           db = context;
        }
        public IActionResult Index()
        {
            IndexVm result = new IndexVm();
            result.Categories = db.Categories.ToList();
            result.Products = db.Products.ToList();
            result.Reviews = db.Reviews.ToList();
            result.ProductImages = db.ProductImages.ToList();
            return View(result);
        }


        [Authorize]
        //public IActionResult AddProductToCart(int Id)
        //{

        //    var price = db.Products.Find(Id).Price;

        //    var item = db.Carts.FirstOrDefault(x => x.ProductId == Id && x.UserId == User.Identity.Name);
        //    if (item != null)
        //    {
        //        item.Qty += 1;
        //        //item.Price += price;
        //    }
        //    else
        //        db.Carts.Add(new Cart { ProductId = Id, UserId = User.Identity.Name, Qty = 1, Price = price });
        //    db.SaveChanges();


        //    return RedirectToAction("Cart");
        //}

        [HttpGet]
        public IActionResult AddProductToCart(int id)
        {
            var product = db.Products.Find(id);
            var price = db.Products.Find(id).Price;
            if (SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session,"cart")==null) {
                shoppingcarts.Add(new Cart
                {
                    ProductId = product.ProductId,
                    UserId= User.Identity.Name,
                    Qty = 1,
                    Price = price,
                    Product=product,
                    
                }) ; 

                SessionHelpers.SetObjectAsJson(HttpContext.Session, "cart", shoppingcarts);
                db.Carts.Add(new Cart
                {
                    ProductId = product.ProductId,
                    UserId = User.Identity.Name,
                    Qty = 1,
                    Price = price,
                    Product = product,
                });
                db.SaveChanges();
            }
            else {
                List<Cart> shoppingcarts = SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
                int index = exists(id, shoppingcarts);
                if (index==-1)
                {
                    shoppingcarts.Add(new Cart
                    {
                        ProductId = product.ProductId,
                        UserId = User.Identity.Name,
                        Qty = 1,
                        Price = price,
                        Product = product,

                    });
                }
                else
                {
                    int newquty = shoppingcarts[index].Qty++;
                    shoppingcarts[index].Qty = newquty;

                }
                SessionHelpers.SetObjectAsJson(HttpContext.Session, "cart", shoppingcarts);
               }
              return RedirectToAction("Cart");

        }
        [HttpPost]
        public IActionResult AddProductToCart(int id, int quantity)
        {
            var product = db.Products.Find(id);
            var price = db.Products.Find(id).Price;
            if (SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart") == null)
            {
                shoppingcarts.Add(new Cart
                {
                    ProductId = product.ProductId,
                    UserId = User.Identity.Name,
                    Qty = quantity,
                    Price = price,
                    Product = product,

                });

                SessionHelpers.SetObjectAsJson(HttpContext.Session, "cart", shoppingcarts);
                db.Carts.Add(new Cart
                {
                    ProductId = product.ProductId,
                    UserId = User.Identity.Name,
                    Qty = quantity,
                    Price = price,
                    Product = product,
                });
                db.SaveChanges();
            }
            else
            {
                List<Cart> shoppingcarts = SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
                int index = exists(id, shoppingcarts);
                if (index == -1)
                {
                    shoppingcarts.Add(new Cart
                    {
                        ProductId = product.ProductId,
                        UserId = User.Identity.Name,
                        Qty = quantity,
                        Price = price,
                        Product = product,

                    });
                }
                else

                {
                    // int newquty = shoppingcarts[index].Qty++;
                    shoppingcarts[index].Qty += quantity;

                }
                SessionHelpers.SetObjectAsJson(HttpContext.Session, "cart", shoppingcarts);

            }
            return RedirectToAction("Cart");
        }

        public IActionResult Update(int[] quantity)
        {

            List<Cart> shoppingcarts = SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            for (var i = 0; i < shoppingcarts.Count; i++)
            {

                shoppingcarts[i].Qty= quantity[i];

            }

            SessionHelpers.SetObjectAsJson(HttpContext.Session, "cart", shoppingcarts);

            return RedirectToAction("Cart");

        }
            public ApplicationDbContext GetDb()
        {
            return db;
        }

        private int exists(int id,List<Cart> carts)
        {
            for (var i = 0; i < carts.Count; i++)
            {
                if (carts[i].CartId==id)
                {
                    return i;
                }

            }
            return -1;
        }
        public IActionResult Cart()
        {
            //var result = db.Carts.Include(x => x.Product).Where(x => x.UserId == User.Identity.Name).ToList();
            List<Cart> shoppingcarts = SessionHelpers.GetObjectFromJson<List<Cart>>(HttpContext.Session, "cart");
            ViewBag.cart = shoppingcarts;
            ViewBag.countitems = shoppingcarts.Count;
            ViewBag.Total = shoppingcarts.Sum(it => it.Price * it.Qty);
            return View();
        }


        [HttpPost]
     //   public IActionResult Cart(Cart2 Carts)
     //   {
     //       if (ModelState.IsValid)
     //       {
     //           db.Carts.Update(Carts);
     //           db.SaveChanges();
     //           return RedirectToAction(nameof(Index));

     //       }
     //       return View(Carts);
     //}
        [Authorize]
        public IActionResult Remove(int? id)
        {
            if (db.Carts == null)
            {
                return Problem("Entity set 'MersalApp2023_StoreContext.Carts'  is null.");
            }
            var cartitem = db.Carts.Find(id);
            if (cartitem != null)
            {
                db.Carts.Remove(cartitem);
            }

            db.SaveChanges();

            return RedirectToAction(nameof(Cart));
        }

        
        public IActionResult CurrentProduct(int Id)
        {
            var pro = db.Products.Include(p => p.ProductImages).FirstOrDefault(x => x.ProductId == Id);
            //ViewBag.CatName = db.Categories.Find(pro.CategoryId).CategoryName;
            //ViewBag.relatedProduct = db.Products.Where(x => x.CategoryId == pro.CategoryId && (x.Price > pro.Price || x.Price < pro.Price));
            return View(pro);
        }
    
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult contact()
        {
            return View("contact");
        }


        [HttpPost]

        public IActionResult contact(Contact model)
        {
          // await _emailSender.SendEmailAsync("asmaa.ali.kashef@gmail.com",model.Name,model.Description);
           MailMessage mail = new MailMessage();
            // you need to enter your mail address
            mail.From = new MailAddress("asmaa.ali.kashef@gmail.com");
            //To Email Address - your need to enter your to email address
            mail.To.Add("asmaa.ali.kashef@gmail.com");
            mail.Subject = model.Subject;
            mail.IsBodyHtml = true;
            string content = "Name : " + model.Name;
            string phone = "phone: " + model.Phone;
            content += "<br/> Message : " + model.Description;
            mail.Body = content;
            SmtpClient smtpClient = new SmtpClient("Smtp.Gmail.com");
            //Create nerwork credential and you need to give from email address and password
            NetworkCredential networkCredential = new NetworkCredential("asmaa.ali.kashef@gmail.com", "trcejhtyrowwdqsa");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Port = 587; // this is default port number - you can also change this
            smtpClient.EnableSsl = true; // if ssl required you need to enable it
            smtpClient.Send(mail);
           db.contacts.Add(new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Subject = model.Subject,
                    Description = model.Description
                });
                db.SaveChanges();
                return View("contact");
            }


        
    
  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}