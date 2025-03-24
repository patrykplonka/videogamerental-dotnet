using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Services;

namespace WypozyczalniaGier.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly IUzytkownikService _uzytkownikService;

        public UzytkownikController(IUzytkownikService uzytkownikService)
        {
            _uzytkownikService = uzytkownikService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var (items, totalCount) = await _uzytkownikService.GetPaginatedAsync(pageNumber, pageSize);
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
        public async Task<IActionResult> Create(Uzytkownik uzytkownik)
        {
            uzytkownik.Wypozyczenia = uzytkownik.Wypozyczenia ?? new List<Wypozyczenie>();

            if (ModelState.ContainsKey("Wypozyczenia"))
            {
                ModelState["Wypozyczenia"].Errors.Clear();
                ModelState["Wypozyczenia"].ValidationState =
                    Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return View(uzytkownik);

            try
            {
                await _uzytkownikService.AddAsync(uzytkownik);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas dodawania użytkownika: {ex.Message}");
                return View(uzytkownik);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
            if (uzytkownik == null)
                return NotFound();

            return View(uzytkownik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Uzytkownik uzytkownik)
        {
            if (id != uzytkownik.IdUzytkownika)
                return BadRequest();

            if (ModelState.ContainsKey("Wypozyczenia"))
            {
                ModelState["Wypozyczenia"].Errors.Clear();
                ModelState["Wypozyczenia"].ValidationState =
                    Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return View(uzytkownik);

            try
            {
                await _uzytkownikService.UpdateAsync(uzytkownik);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas edytowania użytkownika: {ex.Message}");
                return View(uzytkownik);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
            if (uzytkownik == null)
                return NotFound();

            return View(uzytkownik);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
                if (uzytkownik == null)
                    return NotFound();

                await _uzytkownikService.RemoveAsync(uzytkownik);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas usuwania użytkownika: {ex.Message}");
                var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
                return View("Delete", uzytkownik);
            }
        }

        // Dodatkowe akcje wykorzystujące własne metody z serwisu

        [HttpGet]
        public async Task<IActionResult> Search(string lastName)
        {
            var results = await _uzytkownikService.SearchByLastNameAsync(lastName ?? "");
            return View("Index", results);
        }

        [HttpGet]
        public async Task<IActionResult> ActiveUsersCount()
        {
            var count = await _uzytkownikService.GetActiveUsersCountAsync();
            return Json(new { activeUsersCount = count });
        }

        [HttpGet]
        public async Task<IActionResult> CheckBorrowingStatus(int id)
        {
            var hasBorrowedGames = await _uzytkownikService.HasBorrowedGamesAsync(id);
            return Json(new { hasBorrowedGames });
        }
    }
}