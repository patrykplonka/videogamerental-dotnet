using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WypozyczalniaGier.Data;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Repositories
{
    public class GraRepository : IGraRepository
    {
        private readonly ApplicationDbContext _context;

        public GraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Gra> GetAllAsync()
        {
            return _context.Gry.AsQueryable();
        }

        public Task<Gra> GetByIdAsync(int id)
        {
            return _context.Gry.FindAsync(id).AsTask();
        }

        public async Task AddAsync(Gra gra)
        {
            _context.Gry.Add(gra);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Gra gra)
        {
            _context.Entry(gra).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gra = await _context.Gry.FindAsync(id);
            if (gra != null)
            {
                _context.Gry.Remove(gra);
                await _context.SaveChangesAsync();
            }
        }
    }
}