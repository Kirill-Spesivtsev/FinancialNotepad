using System.Globalization;
using FinancialNotepad.Data;
using FinancialNotepad.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialNotepad.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public TransactionController(ApplicationDbContext context, ILogger<TransactionController> logger)
        {
            _context = context;
            _logger = logger;
        }

         // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var list = 
                _context.Transactions
                    .Include(t => t.Category)
                    .Include(t => t.Currency)
                    .Include(t => t.Tax);

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .ToListAsync();

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

            //var calendarProfit = _context.Transactions.GroupBy(t => t.Date).Select(t => new {t.Key, t.Sum(t.Value)}));

            Task.Run(() => FillCalendarProfits());

            return View(list.OrderBy(t => t.Date).ToList());
        }

        // GET: Transaction/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            
            FillSelects();
            if (id == 0)
                return View(new Transaction{TransactionId = 0});
            else
                return View(_context.Transactions.Find(id));
        }

        // POST: Transaction/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit
                ([Bind("TransactionId,CategoryId,CurrencyId,TaxId,Amount,Note,Date,Type")] 
                Transaction transaction)
        {
            ModelState.Clear();
            //ModelState.Remove("CategoryTitleAndIcon");
            
            
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
        public void FillCalendarProfits()
        {
            var calendarProfits = from p in _context.Transactions 
                group p by p.Date into g
                select new{StartTime = g.Key, EndTime = g.Key, 
                    Subject = g.Sum(item=> item.Type == "Expense"? -item.Amount : item.Amount )} ;
            ViewBag.CalendarProfits = calendarProfits;
        }

        [NonAction]
        public void FillSelects()
        {
            var taxes = _context.Taxes.ToList();
            ViewBag.Taxes = taxes; 
            var currencies = _context.Currencies.ToList();
            ViewBag.Currencies = currencies; 
            var categories = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            categories.Insert(0, DefaultCategory);
            ViewBag.Categories = categories;
        }

    }
}
