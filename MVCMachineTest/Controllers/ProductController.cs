using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;  // <-- Add this line
using MVCMachineTest.Models;
using System.Linq;
using System;

namespace MVCMachineTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Index
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            // Calculate total product count
            var totalProducts = _context.Products.Count();

            // Get paginated list of products including category names
            var products = _context.Products
                .OrderBy(p => p.ProductId)  // Sorting by ProductId
                .Skip((page - 1) * pageSize)  // Skip products already displayed
                .Take(pageSize)  // Take only the page size number of products
                .Include(p => p.Category)  // Include the Category details
                .ToList();

            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Pass pagination and products to the view
            ViewBag.TotalProducts = totalProducts;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);  // Pass the list of products as model
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            // Create a SelectList for the Categories and pass it to the view via ViewBag
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Pass Categories and the selected CategoryId back to the view in case of a validation failure
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            // Pass Categories and the selected CategoryId to the view
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Pass Categories and the selected CategoryId back to the view in case of a validation failure
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Details/5
        public IActionResult Details(int id)
        {
            var product = _context.Products
                                   .Where(p => p.ProductId == id)
                                   .Select(p => new
                                   {
                                       p.ProductId,
                                       p.ProductName,
                                       CategoryName = p.Category.CategoryName
                                   })
                                   .FirstOrDefault();
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
