using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialNotepad.Controllers
{
    public class TaxController : Controller
    {
        // GET: TaxController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TaxController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaxController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaxController/Create
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

        // GET: TaxController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaxController/Edit/5
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

        // GET: TaxController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaxController/Delete/5
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
