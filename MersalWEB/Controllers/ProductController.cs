using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MersalWEB.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment _env;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            db = context;
            _env = env;
        }

        // GET: Admin/Products
        public IActionResult Index()
        {

            var Pro = db.Products.ToList();

            return View(Pro);
        }

        public IActionResult Create()
        {
            ProImagesVm vm = new ProImagesVm();
            ViewData["CatName"] = new SelectList(db.Categories, "CategoryName", "CategoryName");
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(ProImagesVm vm)
        {

            string FileName = UploadFile(vm.prophoto);
            vm.product.Photo = FileName;
            var Cat = db.Categories.FirstOrDefault(x => x.CategoryName == vm.CatName);
            var CatId = Cat.CategoryId;
            vm.product.CategoryId = CatId;
            if (vm.product.IsAvailable == true)
            {
                vm.product.IsAvailable = true;
            }
            else
            {
                vm.product.IsAvailable = false;
            }

            db.Products.Add(vm.product);

            db.SaveChanges();
            if (vm.proImgsVm.Count > 0)
            {

                foreach (var item in vm.proImgsVm)
                {
                    string strigFileName = UploadFile(item);

                    var ProductImages = new ProductImage
                    {
                        Image = strigFileName,
                        ProductId = vm.product.ProductId

                    };

                    db.ProductImages.Add(ProductImages);

                }

                db.SaveChanges();
            }



            TempData["save"] = "Product has been Saved";


            return RedirectToAction(nameof(Index));
        }



        private string UploadFile(IFormFile file)
        {
            string filename = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "admin/assets/images");
                filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filepath = Path.Combine(uploadDir, filename);
                var filestream = new FileStream(filepath, FileMode.Create);

                file.CopyToAsync(filestream);


            }
            return filename;
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || db.Products == null)
            {
                return NotFound();
            }

            var product = db.Products.Find(id);
            ProImagesVm vm = new ProImagesVm();
            vm.product = product;
            vm.productimages = db.ProductImages.Where(pm => pm.ProductId == id).ToList();

            vm.CatName = db.Categories.Find(product.CategoryId).CategoryName;
            if (product == null)
            {
                return NotFound();
            }
            //var listcat = new SelectList(db.Categories, "CategoryName", "CategoryName");
            //foreach (var item in listcat)
            //{
            //    if(item.categoryName==CatName)
            //}

            return View(vm); 
        }

        public IActionResult Delete(int? id)
        {
            if (db.Products == null)
            {
                return Problem("Entity set 'MersalApp2023_StoreContext.Categories'  is null.");
            }
            var product = db.Products.Find(id);

            if (product != null)
            {
                var productImgs = db.ProductImages.Where(pm => pm.ProductId == id).ToList();
                foreach (var item in productImgs)
                {
                    db.ProductImages.Remove(item);
                }
                db.SaveChanges();
                db.Products.Remove(product);
            }

            db.SaveChanges();
            TempData["save"] = "Product has been Deleted";
            return RedirectToAction(nameof(Index));
        }

    }
}
