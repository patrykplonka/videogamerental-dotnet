using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Models;
using WypozyczalniaGier.Repositories;

namespace WypozyczalniaGier.Services
{
    public class UzytkownikService : IUzytkownikService
    {
        private readonly IUzytkownikRepository _uzytkownikRepository;

        public UzytkownikService(IUzytkownikRepository uzytkownikRepository)
        {
            _uzytkownikRepository = uzytkownikRepository;
        }

        public async Task<List<Uzytkownik>> GetAllAsync() => await _uzytkownikRepository.GetAllAsync().ToListAsync();
        public async Task<Uzytkownik> GetByIdAsync(int id) => await _uzytkownikRepository.GetByIdAsync(id);
        public async Task AddAsync(Uzytkownik uzytkownik) => await _uzytkownikRepository.AddAsync(uzytkownik);
        public async Task UpdateAsync(Uzytkownik uzytkownik) => await _uzytkownikRepository.UpdateAsync(uzytkownik);
        public async Task RemoveAsync(Uzytkownik uzytkownik) => await _uzytkownikRepository.RemoveAsync(uzytkownik);

        public async Task<(List<Uzytkownik> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _uzytkownikRepository.GetAllAsync();
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }

        public async Task<List<Uzytkownik>> SearchByLastNameAsync(string lastName)
        {
            return await _uzytkownikRepository.GetAllAsync().Where(u => u.Nazwisko.Contains(lastName)).ToListAsync();
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _uzytkownikRepository.GetAllAsync().CountAsync(u => u.Wypozyczenia.Any());
        }

        public async Task<bool> HasBorrowedGamesAsync(int userId)
        {
            return await _uzytkownikRepository.GetAllAsync()
                .Where(u => u.IdUzytkownika == userId)
                .AnyAsync(u => u.Wypozyczenia.Any(w => w.DataZwrotu == null));
        }
    }
}