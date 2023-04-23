using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MersalCart.Controllers
{
    public class OrderDetailsController1 : Controller
    {
        private readonly ApplicationDbContext DB;
        public OrderDetailsController1(ApplicationDbContext context) {
            DB = context;
              }
        public IActionResult Index()
        {
            var Result = DB.OrderDetails.ToList();
            return View(Result);
        }

        public IActionResult Create()
        {
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderDetail orderDetail)
        {

            if(ModelState.IsValid)
            {
                DB.OrderDetails.Add(orderDetail);
                DB.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        public IActionResult Edit(int? id)
        {

            var result=DB.OrderDetails.Find(id);

            return View("Create",result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderDetail orderDetail)
        {

            if (ModelState.IsValid)
            {
                DB.OrderDetails.Update(orderDetail);
                DB.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(orderDetail);
        }

        public IActionResult Delete(int ? id)
        {

                var result = DB.OrderDetails.Find(id);
                if(result != null)
                {
                    DB.OrderDetails.Remove(result);
                    DB.SaveChanges();

                }

                return View("Create", result);
            }

           
        }
    }

