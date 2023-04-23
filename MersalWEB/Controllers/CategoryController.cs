using MersalWEB.Data;
using MersalWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MersalWEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            db = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var result = db.Categories.ToList();
            return View(result);
        }
        public IActionResult Create()
        {

            CatVm CVm = new CatVm();
            return View(CVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CatVm vm)
        {


            string FileName = UploadFile(vm.CatImgVm);
            vm.category.Photo = FileName;
            if (ModelState.IsValid)
            {

                db.Categories.Add(vm.category);
                db.SaveChanges();
                TempData["save"] = "Category has been Saved";
                return RedirectToAction("Index");
            }
            return View();
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
            if (id == null || db.Categories == null)
            {
                return NotFound();
            }

            var category = db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }
            CatVm vm = new CatVm();
            vm.category = category;

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CatVm vm)
        {
            var obj = db.Categories.Where(mId => mId.CategoryId == vm.category.CategoryId).FirstOrDefault();

            if (id != vm.category.CategoryId)
            {
                return NotFound();
            }

            if (vm.CatImgVm != null)
            {
                string FileName = UploadFile(vm.CatImgVm);
                vm.category.Photo = FileName;
            }
            else
            {
                vm.category.Photo = obj.Photo;
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (vm.category != null)
                    {
                        //Update the existing member with new value
                        obj.CategoryName = vm.category.CategoryName;
                        obj.CategoryDescription = vm.category.CategoryDescription;
                        obj.Photo = vm.category.Photo;


                        db.SaveChanges();
                    }

                    TempData["save"] = "Category has been Updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(vm.category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm.category);
        }

        public IActionResult Delete(int? id)
        {
            if (db.Categories == null)
            {
                return Problem("Entity set 'MersalApp2023_StoreContext.Categories'  is null.");
            }
            var category = db.Categories.Find(id);
            if (category != null)
            {
                db.Categories.Remove(category);
            }

            db.SaveChanges();
            TempData["save"] = "Category has been Deleted";
            return RedirectToAction(nameof(Index));
        }
        private bool CategoryExists(int CategoryId)
        {
            throw new NotImplementedException();
        }

    }
}
