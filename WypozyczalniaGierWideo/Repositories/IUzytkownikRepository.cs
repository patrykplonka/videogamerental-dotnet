using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public interface IUzytkownikRepository
    {
        IQueryable<Uzytkownik> GetAllAsync();
        Task<Uzytkownik> GetByIdAsync(int id);
        Task AddAsync(Uzytkownik uzytkownik);
        Task UpdateAsync(Uzytkownik uzytkownik);
        Task RemoveAsync(Uzytkownik uzytkownik);
    }
}