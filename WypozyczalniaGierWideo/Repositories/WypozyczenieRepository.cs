using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Data;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public class WypozyczenieRepository : IWypozyczenieRepository
    {
        private readonly ApplicationDbContext _context;

        public WypozyczenieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Wypozyczenie> GetAllAsync()
        {
            return _context.Wypozyczenia
                .Include(w => w.Uzytkownik)
                .Include(w => w.Gra);
        }

        public async Task<Wypozyczenie> GetByIdAsync(int id)
        {
            return await _context.Wypozyczenia
                .Include(w => w.Uzytkownik)
                .Include(w => w.Gra)
                .FirstOrDefaultAsync(w => w.IdWypozyczenia == id);
        }

        public async Task AddAsync(Wypozyczenie wypozyczenie)
        {
            _context.Wypozyczenia.Add(wypozyczenie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wypozyczenie wypozyczenie)
        {
            _context.Entry(wypozyczenie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wypozyczenie = await _context.Wypozyczenia.FindAsync(id);
            if (wypozyczenie != null)
            {
                _context.Wypozyczenia.Remove(wypozyczenie);
                await _context.SaveChangesAsync();
            }
        }
    }
}