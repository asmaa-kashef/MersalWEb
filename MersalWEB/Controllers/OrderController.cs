using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MersalCart.Controllers
{
    public class OrderController : Controller

    {
        private readonly ApplicationDbContext DB;
        public OrderController(ApplicationDbContext context)
        {

            DB = context;
        }
        public IActionResult Index()
        {
            var Result = DB.Orders.Include(x => x.Orderdetails).Include(x => x.Product).ToList();
            return View(Result);
        }
        public IActionResult Create()
        {
            IndexVm result = new IndexVm();
            result.products = DB.Products.ToList();
            result.OrderDetails = DB.OrderDetails.ToList();
            return View(result);
        }
        
    }
}