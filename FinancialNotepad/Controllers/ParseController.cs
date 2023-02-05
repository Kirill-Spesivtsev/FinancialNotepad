using FinancialNotepad.ViewModels;
using MathExpressionConstructor;
using Microsoft.AspNetCore.Mvc;

namespace FinancialNotepad.Controllers
{
    public class ParseController : Controller
    {
        public IMathExprConstructor _expParser = new ParserMain();

        public ParseController(IMathExprConstructor parser)
        {
            _expParser = parser;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Parse(TextBlockModel tb)
        {
            string init = tb.Text;
            var res = _expParser.Parse(init);
            ViewBag.ExprResult = res;
            return View("Index",tb);
        }
    }
}
