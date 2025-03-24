using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public interface IWypozyczenieRepository
    {
        IQueryable<Wypozyczenie> GetAllAsync();
        Task<Wypozyczenie> GetByIdAsync(int id);
        Task AddAsync(Wypozyczenie wypozyczenie);
        Task UpdateAsync(Wypozyczenie wypozyczenie);
        Task DeleteAsync(int id);
    }
}