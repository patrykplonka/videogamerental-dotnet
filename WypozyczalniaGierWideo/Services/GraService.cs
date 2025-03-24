using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Repositories;

namespace WypozyczalniaGier.Services
{
    public class GraService : IGraService
    {
        private readonly IGraRepository _graRepository;

        public GraService(IGraRepository graRepository)
        {
            _graRepository = graRepository;
        }

        public async Task<List<Gra>> GetAllAsync() => await _graRepository.GetAllAsync().ToListAsync();
        public async Task<Gra> GetByIdAsync(int id) => await _graRepository.GetByIdAsync(id);
        public async Task AddAsync(Gra gra) => await _graRepository.AddAsync(gra);
        public async Task UpdateAsync(Gra gra) => await _graRepository.UpdateAsync(gra);
        public async Task DeleteAsync(int id) => await _graRepository.DeleteAsync(id);

        public async Task<(List<Gra> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _graRepository.GetAllAsync();
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }

        public async Task<List<Gra>> SearchByTitleAsync(string title)
        {
            return await _graRepository.GetAllAsync().Where(g => g.Tytul.Contains(title)).ToListAsync();
        }

        public async Task<List<Gra>> GetMostBorrowedAsync(int topN)
        {
            return await _graRepository.GetAllAsync()
                .OrderByDescending(g => g.Wypozyczenia.Count)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<bool> IsAvailableAsync(int graId)
        {
            return !await _graRepository.GetAllAsync()
                .Where(g => g.IdGry == graId)
                .AnyAsync(g => g.Wypozyczenia.Any(w => w.DataZwrotu == null));
        }
    }
}