using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MersalCart.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext DB;

        public InvoiceController(ApplicationDbContext context)
        {
            DB = context;
        }
        public IActionResult Index()
        {
            var Result = DB.Invoices.Include(x => x.Product).ToList();
            return View(Result);

        }
        public IActionResult Create()
        {
            ViewBag.product = DB.Products.OrderBy(x => x.Name).ToList();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Invoice invoices)
        {
            if (ModelState.IsValid)
            {
                DB.Invoices.Add(invoices);
                DB.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.product = DB.Products.OrderBy(x => x.Name).ToList();
            return View();
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.product = DB.Products.OrderBy(x => x.Name).ToList();
            var result = DB.Invoices.Find(id);
            return View("Edit", result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Invoice invoices)
        {
            if (ModelState.IsValid)
            {
                DB.Invoices.Update(invoices);
                DB.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            ViewBag.product = DB.Products.OrderBy(x => x.Name).ToList();
            return View(invoices);

        }
        public IActionResult Delete(int? id)
        {

            var result = DB.Invoices.Find(id);
            if (result != null)
            {
                DB.Invoices.Remove(result);
                DB.SaveChanges();

            }

            return View("Create", result);
        }


    }
}

