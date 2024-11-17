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

        // GET: Category/Index
        public IActionResult Index(int page = 1)
        {
            int pageSize = 10; // Set the page size (10 categories per page)

            // Get the total number of categories
            int totalCategories = _context.Categories.Count();

            // Calculate total pages based on the total categories and page size
            int totalPages = (int)Math.Ceiling((double)totalCategories / pageSize);

            // Get the categories for the current page
            var categories = _context.Categories
                                    .OrderBy(c => c.CategoryId)  // Order categories by CategoryId (or any other field)
                                    .Skip((page - 1) * pageSize)  // Skip the categories for the previous pages
                                    .Take(pageSize)              // Take the number of categories for the current page
                                    .ToList();

            // Pass necessary data to the view
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Return the view with the categories data
            return View(categories);
        }


        // GET: Category/Create
        public IActionResult Create() => View();

        // POST: Category/Create
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

        // GET: Category/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Category/Edit/5
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

        // GET: Category/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Category/Delete/5
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

        // GET: Category/Details/5
        public IActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }
    }
}
