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
    public class CreditController : Controller
    {
        public ApplicationDbContext _context;
        public ILogger<CreditController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CreditController(ApplicationDbContext context, ILogger<CreditController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }

        // GET: CreditController
        public ActionResult Index()
        {
            var _userId = _userManager.GetUserId(User);
            ViewBag.UserId = _userId;
            //var crts =_context.Credits.Where(q => q.UserId == _userId).Include(t => t.Currency);

            return View(/*crts.ToList()*/);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            FillSelects();
            if (id == 0)
            {
                return View(new Credit{CreditId = 0});
            }

            else
            {
                //var cr = _context.Credits.Find(id);
                return View(/*cr*/);
            }
                
        }

        // POST: Transaction/AddOrEdit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit
        ([Bind("CreditId,Description,Status,Type,TotalSum,CurrencyId,AnnualPercent,Period,LeftToPay,UserId")] 
            Credit credit)
        {
            
            ModelState.Clear();
            var userId = _userManager.GetUserId(User);
            credit.UserId = userId;


            if (ModelState.IsValid)
            {
                if (credit.CreditId == 0)
                    _context.Add(credit);
                else
                    _context.Update(credit);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            FillSelects();
            
            return View(credit);
        }

        // POST: Credit/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [NonAction]
        public void FillSelects()
        {
            var currencies = _context.Currencies.ToList();
            ViewBag.Currencies = currencies;
        }

        
    }
}
