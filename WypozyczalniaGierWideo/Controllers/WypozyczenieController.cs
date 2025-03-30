using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.ViewModels;
using WypozyczalniaGier.Services;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Repositories;
using WypozyczalniaGier.Models;

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
            var wypozyczenia = (await _wypozyczenieService.GetAllAsync())
                .Select(w => new WypozyczenieViewModel
                {
                    IdWypozyczenia = w.IdWypozyczenia,
                    IdUzytkownika = w.Uzytkownik.IdUzytkownika,
                    UzytkownikNazwa = w.Uzytkownik.Imie + " " + w.Uzytkownik.Nazwisko,
                    IdGry = w.Gra.IdGry,
                    GraTytul = w.Gra.Tytul,
                    DataWypozyczenia = w.DataWypozyczenia,
                    DataZwrotu = w.DataZwrotu,
                    DataZwrotuRzeczywista = w.DataZwrotuRzeczywista,
                    Kara = w.Kara,
                    Koszt = w.Koszt
                }).ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                wypozyczenia = wypozyczenia
                    .Where(w => w.UzytkownikNazwa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                w.GraTytul.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

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
        public async Task<IActionResult> Create(WypozyczenieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(model);
            }

            var wypozyczenie = new Wypozyczenie
            {
                IdUzytkownika = model.IdUzytkownika,
                IdGry = model.IdGry,
                DataWypozyczenia = model.DataWypozyczenia,
                DataZwrotu = model.DataZwrotu,
                Kara = model.Kara,
                Koszt = model.Koszt
            };

            await _wypozyczenieService.AddAsync(wypozyczenie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var wypozyczenie = await _wypozyczenieService.GetByIdAsync(id);
            if (wypozyczenie == null) return NotFound();

            var model = new WypozyczenieViewModel
            {
                IdWypozyczenia = wypozyczenie.IdWypozyczenia,
                IdUzytkownika = wypozyczenie.IdUzytkownika,
                IdGry = wypozyczenie.IdGry,
                DataWypozyczenia = wypozyczenie.DataWypozyczenia,
                DataZwrotu = wypozyczenie.DataZwrotu,
                DataZwrotuRzeczywista = wypozyczenie.DataZwrotuRzeczywista,
                Kara = wypozyczenie.Kara,
                Koszt = wypozyczenie.Koszt
            };

            ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
            ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WypozyczenieViewModel model)
        {
            if (id != model.IdWypozyczenia) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Uzytkownicy = await _uzytkownikRepository.GetAllAsync().ToListAsync();
                ViewBag.Gry = await _graRepository.GetAllAsync().ToListAsync();
                return View(model);
            }

            var wypozyczenie = await _wypozyczenieService.GetByIdAsync(id);
            if (wypozyczenie == null) return NotFound();

            wypozyczenie.IdUzytkownika = model.IdUzytkownika;
            wypozyczenie.IdGry = model.IdGry;
            wypozyczenie.DataWypozyczenia = model.DataWypozyczenia;
            wypozyczenie.DataZwrotu = model.DataZwrotu;
            wypozyczenie.DataZwrotuRzeczywista = model.DataZwrotuRzeczywista;
            wypozyczenie.Kara = model.Kara;
            wypozyczenie.Koszt = model.Koszt;

            await _wypozyczenieService.UpdateAsync(wypozyczenie);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _wypozyczenieService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
