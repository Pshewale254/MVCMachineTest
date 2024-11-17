using Microsoft.AspNetCore.Mvc;
using MVCMachineTest.Models;
using System.Linq;

namespace MVCMachineTest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index(int page = 1)
        {
            int pageSize = 10; 

           
            int totalCategories = _context.Categories.Count();

            
            int totalPages = (int)Math.Ceiling((double)totalCategories / pageSize);

           
            var categories = _context.Categories
                                    .OrderBy(c => c.CategoryId)  
                                    .Skip((page - 1) * pageSize)  
                                    .Take(pageSize)              
                                    .ToList();

           
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

           
            return View(categories);
        }


        
        public IActionResult Create() => View();

        
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

       
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

       
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

  
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }
    }
}
