using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Data;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public class UzytkownikRepository : IUzytkownikRepository
    {
        private readonly ApplicationDbContext _context;

        public UzytkownikRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Uzytkownik> GetAllAsync()
        {
            return _context.Uzytkownicy;
        }

        public async Task<Uzytkownik> GetByIdAsync(int id)
        {
            return await _context.Uzytkownicy.FindAsync(id);
        }

        public async Task AddAsync(Uzytkownik uzytkownik)
        {
            _context.Uzytkownicy.Add(uzytkownik);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Uzytkownik uzytkownik)
        {
            _context.Entry(uzytkownik).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Uzytkownik uzytkownik)
        {
            _context.Uzytkownicy.Remove(uzytkownik);
            await _context.SaveChangesAsync();
        }
    }
}