using System.Collections.Generic;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Services
{
    public interface IWypozyczenieService
    {
        Task<List<Wypozyczenie>> GetAllAsync();
        Task<Wypozyczenie> GetByIdAsync(int id);
        Task AddAsync(Wypozyczenie wypozyczenie);
        Task UpdateAsync(Wypozyczenie wypozyczenie);
        Task DeleteAsync(int id);
        Task<(List<Wypozyczenie> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<List<Wypozyczenie>> GetByUserIdAsync(int userId);
        Task<List<Wypozyczenie>> GetOverdueAsync();
        Task<bool> IsGameBorrowedAsync(int graId);
    }
}