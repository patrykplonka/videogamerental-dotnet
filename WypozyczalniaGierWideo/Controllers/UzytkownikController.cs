using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Services;
using WypozyczalniaGier.ViewModels;

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
            var viewModelItems = items.Select(u => new UzytkownikViewModel
            {
                IdUzytkownika = u.IdUzytkownika,
                Imie = u.Imie,
                Nazwisko = u.Nazwisko,
                Email = u.Email
            }).ToList();

            ViewBag.TotalCount = totalCount;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(viewModelItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UzytkownikViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UzytkownikViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var uzytkownik = new Uzytkownik
            {
                IdUzytkownika = viewModel.IdUzytkownika,
                Imie = viewModel.Imie,
                Nazwisko = viewModel.Nazwisko,
                Email = viewModel.Email,
                Wypozyczenia = new List<Wypozyczenie>()
            };

            try
            {
                await _uzytkownikService.AddAsync(uzytkownik);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas dodawania użytkownika: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
            if (uzytkownik == null)
                return NotFound();

            var viewModel = new UzytkownikViewModel
            {
                IdUzytkownika = uzytkownik.IdUzytkownika,
                Imie = uzytkownik.Imie,
                Nazwisko = uzytkownik.Nazwisko,
                Email = uzytkownik.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UzytkownikViewModel viewModel)
        {
            if (id != viewModel.IdUzytkownika)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
            if (uzytkownik == null)
                return NotFound();

            uzytkownik.Imie = viewModel.Imie;
            uzytkownik.Nazwisko = viewModel.Nazwisko;
            uzytkownik.Email = viewModel.Email;

            try
            {
                await _uzytkownikService.UpdateAsync(uzytkownik);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas edytowania użytkownika: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var uzytkownik = await _uzytkownikService.GetByIdAsync(id);
            if (uzytkownik == null)
                return NotFound();

            var viewModel = new UzytkownikViewModel
            {
                IdUzytkownika = uzytkownik.IdUzytkownika,
                Imie = uzytkownik.Imie,
                Nazwisko = uzytkownik.Nazwisko,
                Email = uzytkownik.Email
            };

            return View(viewModel);
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
                var viewModel = new UzytkownikViewModel
                {
                    IdUzytkownika = uzytkownik.IdUzytkownika,
                    Imie = uzytkownik.Imie,
                    Nazwisko = uzytkownik.Nazwisko,
                    Email = uzytkownik.Email
                };
                return View("Delete", viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string lastName)
        {
            var results = await _uzytkownikService.SearchByLastNameAsync(lastName ?? "");
            var viewModelResults = results.Select(u => new UzytkownikViewModel
            {
                IdUzytkownika = u.IdUzytkownika,
                Imie = u.Imie,
                Nazwisko = u.Nazwisko,
                Email = u.Email
            }).ToList();

            return View("Index", viewModelResults);
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