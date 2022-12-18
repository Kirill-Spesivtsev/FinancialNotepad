using System.Globalization;
using FinancialNotepad.Data;
using FinancialNotepad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialNotepad.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {

            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
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
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:F}", profit);

            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First()?.Category?.Icon + " " + k.First()?.Category?.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("F") + " $",
                })
                .OrderByDescending(l => l.amount)
                .ToList();


            List<PlotData> incomeStatistics = SelectedTransactions
                .Where(i => i.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new PlotData()
                {
                    day = k.First().Date.ToString("dd-MM-yyyy"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            List<PlotData> expenseStatistics = SelectedTransactions
                .Where(i => i.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new PlotData()
                {
                    day = k.First().Date.ToString("dd-MM-yyyy"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            string[] LastWeekData = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MM-yyyy"))
                .ToArray();

            ViewBag.SplineChartData = 
                    from day in LastWeekData
                    join income in incomeStatistics on day equals income.day into dayIncomeJoined
                    from income in dayIncomeJoined.DefaultIfEmpty()
                    join expense in expenseStatistics on day equals expense.day into expenseJoined
                    from expense in expenseJoined.DefaultIfEmpty()
                    select new
                    {
                        day = day,
                        income = income == null ? 0 : income.income,
                        expense = expense == null ? 0 : expense.expense,
                    };

            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();


            return View();
        }
    }

    public class PlotData
    {
        public string day;
        public double income;
        public double expense;

    }
}
