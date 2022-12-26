using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        
        public TransactionController(ApplicationDbContext context, ILogger<TransactionController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            
        }

        

         // GET: Transaction
         [Authorize]
        public async Task<IActionResult> Index()
        {
            var _userId = _userManager.GetUserId(User);
            ViewBag.UserId = _userId;
            var list =
                _context.Transactions
                    .Where(q => q.UserId == _userId)
                    .Include(t => t.Category)
                    .Include(t => t.Currency)
                    .Include(t => t.Tax);
            

            await Task.Run(() => FillStatus(_userId));

            await Task.Run(() => FillCalendarProfits(_userId));

            return View(list.OrderBy(t => t.Date).ToList());
        }

        // GET: Transaction/AddOrEdit
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            FillSelects();
            if (id == 0)
            {
                return View(new Transaction{TransactionId = 0});
            }

            else
            {
                var tr = _context.Transactions.Find(id);
                return View(tr);
            }
                
        }

        // POST: Transaction/AddOrEdit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit
                ([Bind("TransactionId,CategoryId,CurrencyId,TaxId,Amount,Note,Date,Type,UserId")] 
                Transaction transaction)
        {
            
            ModelState.Clear();
            var userId = _userManager.GetUserId(User);
            transaction.UserId = userId;


            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                    _context.Add(transaction);
                else
                    _context.Update(transaction);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            FillSelects();
            return View(transaction);
        }

        // POST: Transaction/Delete/5
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
        public void FillStatus(string _userId)
        {
            List<Transaction> SelectedTransactions = _context.Transactions
                .Where(q => q.UserId == _userId)
                .Include(x => x.Category)
                .ToList();
            double totalIncome = SelectedTransactions
                .Where(i => i.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("F") + " $";

            double totalExpense = SelectedTransactions
                .Where(i => i.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("F") + " $";

            var profit = totalIncome - totalExpense;

            ViewBag.Profit = profit.ToString("F") + " $";

        }

        [NonAction]
        public void FillCalendarProfits(string _userId)
        {
            var trans = _context.Transactions.Where(q => q.UserId == _userId);
            var calendarProfits = from p in trans
                group p by p.Date into g
                select new{StartTime = g.Key, EndTime = g.Key, 
                    Subject = g.Sum(item=> item.Type == "Expense"? -item.Amount : item.Amount )} ;
            ViewBag.CalendarProfits = calendarProfits;
        }

        [NonAction]
        public void FillSelects()
        {
            var userId = _userManager.GetUserId(User);
            var taxes = _context.Taxes.ToList();
            var currencies = _context.Currencies.ToList();
            var categories = _context.Categories.Where(q => q.UserId == userId).ToList();
            ViewBag.Taxes = taxes;
            ViewBag.Currencies = currencies;
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            categories.Insert(0, DefaultCategory);
            ViewBag.Categories = categories;
        }

    }
}
