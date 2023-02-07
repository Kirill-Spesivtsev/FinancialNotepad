using FinancialNotepad.ViewModels;
using MathExpressionConstructor;
using Microsoft.AspNetCore.Mvc;

namespace FinancialNotepad.Controllers
{
    public class MathConstructorController : Controller
    {
        public IMathExprConstructor _expParser = new ParserMain();
        public IActionResult Index()
        {
            return View(new MathExpression{InputText = ""});
        }

        [HttpPost]
        public async Task<IActionResult> Parse(MathExpression tb)
        {
            string init = tb.InputText;
            var res = _expParser.Parse(init);
            ViewBag.ExprResult = res;
            return View("Index",tb);
        }
    }
}
