using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Services;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Repositories;

namespace WypozyczalniaGier.Controllers
{
    public class WypozyczenieController : Controller
    {
        private readonly IWypozyczenieService _wypozyczenieService;
        private readonly IUzytkownikRepository _uzytkownikRepository;
        private readonly IGraRepository _graRepository;

        public WypozyczenieController(
            IWypozyczenieService wypozyczenieService,
            IUzytkownikRepository uzytkownikRepository,
            IGraRepository graRepository)
        {
            _wypozyczenieService = wypozyczenieService;
            _uzytkownikRepository = uzytkownikRepository;
            _graRepository = graRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var wypozyczenia = await _wypozyczenieService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                wypozyczenia = wypozyczenia
                    .Where(w => w.Uzytkownik.Imie.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                w.Uzytkownik.Nazwisko.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                w.Gra.Tytul.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Przekazanie wartości searchTerm do widoku
            ViewData["searchTerm"] = searchTerm;

            return View(wypozyczenia);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
            ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Wypozyczenie wypozyczenie)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(wypozyczenie);
            }

            try
            {
                await _wypozyczenieService.AddAsync(wypozyczenie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd: {ex.Message}");
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(wypozyczenie);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var wypozyczenie = await _wypozyczenieService.GetByIdAsync(id);
            if (wypozyczenie == null) return NotFound();

            ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
            ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
            return View(wypozyczenie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Wypozyczenie wypozyczenie)
        {
            if (id != wypozyczenie.IdWypozyczenia) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(wypozyczenie);
            }

            try
            {
                await _wypozyczenieService.UpdateAsync(wypozyczenie);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd: {ex.Message}");
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(wypozyczenie);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var wypozyczenie = await _wypozyczenieService.GetByIdAsync(id);
            if (wypozyczenie == null) return NotFound();

            return View(wypozyczenie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _wypozyczenieService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Błąd: {ex.Message}");
                var wypozyczenie = await _wypozyczenieService.GetByIdAsync(id);
                return View("Delete", wypozyczenie);
            }
        }
    }
}
