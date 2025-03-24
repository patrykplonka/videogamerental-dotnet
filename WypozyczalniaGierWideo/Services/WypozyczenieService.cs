using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Repositories;

namespace WypozyczalniaGier.Services
{
    public class WypozyczenieService : IWypozyczenieService
    {
        private readonly IWypozyczenieRepository _wypozyczenieRepository;

        public WypozyczenieService(IWypozyczenieRepository wypozyczenieRepository)
        {
            _wypozyczenieRepository = wypozyczenieRepository;
        }

        public async Task<List<Wypozyczenie>> GetAllAsync() => await _wypozyczenieRepository.GetAllAsync().ToListAsync();
        public async Task<Wypozyczenie> GetByIdAsync(int id) => await _wypozyczenieRepository.GetByIdAsync(id);
        public async Task AddAsync(Wypozyczenie wypozyczenie) => await _wypozyczenieRepository.AddAsync(wypozyczenie);
        public async Task UpdateAsync(Wypozyczenie wypozyczenie) => await _wypozyczenieRepository.UpdateAsync(wypozyczenie);
        public async Task DeleteAsync(int id) => await _wypozyczenieRepository.DeleteAsync(id);

        public async Task<(List<Wypozyczenie> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _wypozyczenieRepository.GetAllAsync();
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }

        public async Task<List<Wypozyczenie>> GetByUserIdAsync(int userId)
        {
            return await _wypozyczenieRepository.GetAllAsync()
                .Where(w => w.IdUzytkownika == userId)
                .ToListAsync();
        }

        public async Task<List<Wypozyczenie>> GetOverdueAsync()
        {
            return await _wypozyczenieRepository.GetAllAsync()
                .Where(w => w.DataZwrotu == null && w.DataWypozyczenia.AddDays(30) < DateTime.Now)
                .ToListAsync();
        }

        public async Task<bool> IsGameBorrowedAsync(int graId)
        {
            return await _wypozyczenieRepository.GetAllAsync()
                .AnyAsync(w => w.IdGry == graId && w.DataZwrotu == null);
        }
    }
}