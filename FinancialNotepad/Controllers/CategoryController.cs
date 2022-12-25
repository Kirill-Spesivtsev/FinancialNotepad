using FinancialNotepad.Data;
using FinancialNotepad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialNotepad.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var categories = _context.Categories.Where(q => q.UserId == userId).ToList();
            return View(categories);

        }


        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category{CategoryId = 0});
            else
                return View(_context.Categories.Find(id));

        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,UserId")] Category category)
        {
            ModelState.Clear();
            //TryValidateModel(category);
            //ModelState.ClearValidationState(nameof(category));

            var userId = _userManager.GetUserId(User);
            category.UserId = userId;

            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    _context.Add(category);
                else
                    _context.Update(category);
 
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("No Items in Categories");
            }
            if (id != 1)
            {
                var category = await _context.Categories.FindAsync(id);
                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
