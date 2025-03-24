using Microsoft.EntityFrameworkCore;
using WypozyczalniaGier.Models;

namespace WypozyczalniaGier.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Gra> Gry { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Wypozyczenie> Wypozyczenia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gra>()
                .HasMany(g => g.Wypozyczenia)
                .WithOne(w => w.Gra)
                .HasForeignKey(w => w.IdGry)
                .IsRequired(false); // Wypożyczenia są opcjonalne
        }
    }
}
