using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Services;
using WypozyczalniaGier.ViewModels;

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
            var viewModels = items.Select(g => new GraViewModel
            {
                IdGry = g.IdGry,
                Tytul = g.Tytul,
                Gatunek = g.Gatunek,
                Platforma = g.Platforma,
                CenaZaDzien = g.CenaZaDzien
            }).ToList();

            ViewBag.TotalCount = totalCount;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GraViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var gra = new Gra
                {
                    Tytul = viewModel.Tytul,
                    Gatunek = viewModel.Gatunek,
                    Platforma = viewModel.Platforma,
                    CenaZaDzien = viewModel.CenaZaDzien,
                    Wypozyczenia = new List<Wypozyczenie>()
                };
                await _graService.AddAsync(gra);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas dodawania gry: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gra = await _graService.GetByIdAsync(id);
            if (gra == null)
                return NotFound();

            var viewModel = new GraViewModel
            {
                IdGry = gra.IdGry,
                Tytul = gra.Tytul,
                Gatunek = gra.Gatunek,
                Platforma = gra.Platforma,
                CenaZaDzien = gra.CenaZaDzien
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GraViewModel viewModel)
        {
            if (id != viewModel.IdGry)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var gra = new Gra
                {
                    IdGry = viewModel.IdGry,
                    Tytul = viewModel.Tytul,
                    Gatunek = viewModel.Gatunek,
                    Platforma = viewModel.Platforma,
                    CenaZaDzien = viewModel.CenaZaDzien
                };
                await _graService.UpdateAsync(gra);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas edytowania gry: {ex.Message}");
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gra = await _graService.GetByIdAsync(id);
            if (gra == null)
                return NotFound();

            var viewModel = new GraViewModel
            {
                IdGry = gra.IdGry,
                Tytul = gra.Tytul,
                Gatunek = gra.Gatunek,
                Platforma = gra.Platforma,
                CenaZaDzien = gra.CenaZaDzien
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
                await _graService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd podczas usuwania gry: {ex.Message}");
                var gra = await _graService.GetByIdAsync(id);
                var viewModel = new GraViewModel
                {
                    IdGry = gra.IdGry,
                    Tytul = gra.Tytul,
                    Gatunek = gra.Gatunek,
                    Platforma = gra.Platforma,
                    CenaZaDzien = gra.CenaZaDzien
                };
                return View("Delete", viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string title)
        {
            var results = await _graService.SearchByTitleAsync(title ?? "");
            var viewModels = results.Select(g => new GraViewModel
            {
                IdGry = g.IdGry,
                Tytul = g.Tytul,
                Gatunek = g.Gatunek,
                Platforma = g.Platforma,
                CenaZaDzien = g.CenaZaDzien
            }).ToList();
            return View("Index", viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> MostBorrowed(int topN = 5)
        {
            var mostBorrowed = await _graService.GetMostBorrowedAsync(topN);
            var viewModels = mostBorrowed.Select(g => new GraViewModel
            {
                IdGry = g.IdGry,
                Tytul = g.Tytul,
                Gatunek = g.Gatunek,
                Platforma = g.Platforma,
                CenaZaDzien = g.CenaZaDzien
            }).ToList();
            return View("Index", viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int id)
        {
            var isAvailable = await _graService.IsAvailableAsync(id);
            return Json(new { available = isAvailable });
        }
    }
}
