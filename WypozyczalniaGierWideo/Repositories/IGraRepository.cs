using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public interface IGraRepository
    {
        IQueryable<Gra> GetAllAsync();
        Task<Gra> GetByIdAsync(int id);
        Task AddAsync(Gra gra);
        Task UpdateAsync(Gra gra);
        Task DeleteAsync(int id);
    }
}