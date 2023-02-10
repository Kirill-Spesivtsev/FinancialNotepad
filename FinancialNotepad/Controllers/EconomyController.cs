using FinancialNotepad.ViewModels;
using MathExpressionConstructor;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace FinancialNotepad.Controllers
{
    public class EconomyController : Controller
    {
        public ParserMain _expParser = new ParserMain();

        public IActionResult Index()
        {
            return View(new EconomyMetrics());
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(EconomyMetrics ec)
        {
            ec.Profit = ec.Income - ec.Expense;
            ec.MrjIncome = ec.Income - ec.VariableCosts;
            ec.PureProfit = ec.Profit * (100 - ec.CommonTaxPercent) / 100;
            ec.AmDed = (double)ec.AmSumCosts / ec.UsefulPeriod;
            return View("Index",ec);
        }

    }
}
