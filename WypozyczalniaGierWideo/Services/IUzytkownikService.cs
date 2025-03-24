using System.Collections.Generic;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Services
{
    public interface IUzytkownikService
    {
        Task<List<Uzytkownik>> GetAllAsync();
        Task<Uzytkownik> GetByIdAsync(int id);
        Task AddAsync(Uzytkownik uzytkownik);
        Task UpdateAsync(Uzytkownik uzytkownik);
        Task RemoveAsync(Uzytkownik uzytkownik);
        Task<(List<Uzytkownik> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<List<Uzytkownik>> SearchByLastNameAsync(string lastName);
        Task<int> GetActiveUsersCountAsync();
        Task<bool> HasBorrowedGamesAsync(int userId);
    }
}