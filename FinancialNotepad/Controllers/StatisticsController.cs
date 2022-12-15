using FinancialNotepad.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinancialNotepad.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
