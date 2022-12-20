using FinancialNotepad.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialNotepad.Controllers
{
    public class CreditController : Controller
    {
        public ApplicationDbContext _context;
        public ILogger<CreditController> _logger;

        public CreditController(ApplicationDbContext context, ILogger<CreditController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: CreditController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreditController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreditController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreditController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreditController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreditController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreditController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreditController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
