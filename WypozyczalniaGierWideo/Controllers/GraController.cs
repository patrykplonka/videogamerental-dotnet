using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Services;

namespace WypozyczalniaGier.Controllers
{
    public class GraController : Controller
    {
        private readonly IGraService _graService;

        public GraController(IGraService graService)
        {
            _graService = graService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var (items, totalCount) = await _graService.GetPaginatedAsync(pageNumber, pageSize);
            ViewBag.TotalCount = totalCount;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gra gra)
        {
            gra.Wypozyczenia = gra.Wypozyczenia ?? new List<Wypozyczenie>();

            // Usuń błędy walidacji dla pola Wypozyczenia
            if (ModelState.ContainsKey("Wypozyczenia"))
            {
                ModelState["Wypozyczenia"].Errors.Clear();
                ModelState["Wypozyczenia"].ValidationState =
                    Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
            {
                return View(gra);
            }

            try
            {
                await _graService.AddAsync(gra);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas dodawania gry: {ex.Message}");
                return View(gra);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gra = await _graService.GetByIdAsync(id);
            if (gra == null)
                return NotFound();

            return View(gra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gra gra)
        {
            if (id != gra.IdGry)
                return BadRequest();

            // Usuń błędy walidacji dla pola Wypozyczenia
            if (ModelState.ContainsKey("Wypozyczenia"))
            {
                ModelState["Wypozyczenia"].Errors.Clear();
                ModelState["Wypozyczenia"].ValidationState =
                    Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return View(gra);

            try
            {
                await _graService.UpdateAsync(gra);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas edytowania gry: {ex.Message}");
                return View(gra);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gra = await _graService.GetByIdAsync(id);
            if (gra == null)
                return NotFound();

            return View(gra);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _graService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas usuwania gry: {ex.Message}");
                var gra = await _graService.GetByIdAsync(id);
                return View("Delete", gra);
            }
        }

        // Dodatkowe akcje wykorzystujące własne metody z serwisu

        [HttpGet]
        public async Task<IActionResult> Search(string title)
        {
            var results = await _graService.SearchByTitleAsync(title ?? "");
            return View("Index", results);
        }

        [HttpGet]
        public async Task<IActionResult> MostBorrowed(int topN = 5)
        {
            var mostBorrowed = await _graService.GetMostBorrowedAsync(topN);
            return View("Index", mostBorrowed);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int id)
        {
            var isAvailable = await _graService.IsAvailableAsync(id);
            return Json(new { available = isAvailable });
        }
    }
}