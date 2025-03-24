using System.Collections.Generic;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Services
{
    public interface IGraService
    {
        Task<List<Gra>> GetAllAsync();
        Task<Gra> GetByIdAsync(int id);
        Task AddAsync(Gra gra);
        Task UpdateAsync(Gra gra);
        Task DeleteAsync(int id);
        Task<(List<Gra> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<List<Gra>> SearchByTitleAsync(string title);
        Task<List<Gra>> GetMostBorrowedAsync(int topN);
        Task<bool> IsAvailableAsync(int graId);
    }
}