using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;  
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

      
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            
            var totalProducts = _context.Products.Count();

           
            var products = _context.Products
                .OrderBy(p => p.ProductId)  
                .Skip((page - 1) * pageSize)  
                .Take(pageSize)  
                .Include(p => p.Category)  
                .ToList();

          
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

           
            ViewBag.TotalProducts = totalProducts;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);  
        }

        public IActionResult Create()
        {
  
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

       
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

           
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

      
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

          
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

      
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

       
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

   
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
