using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WypozyczalniaGier.Models
{
    public class Wypozyczenie
    {
        [Key]
        public int IdWypozyczenia { get; set; }

        [Required]
        public int IdUzytkownika { get; set; }

        [Required]
        public int IdGry { get; set; }

        [Required]
        public DateTime DataWypozyczenia { get; set; } = DateTime.Now;

        [Required]
        public DateTime DataZwrotu { get; set; }

        public DateTime? DataZwrotuRzeczywista { get; set; }

        public decimal Kara { get; set; } = 0.00m;

        [Required]
        public decimal Koszt { get; set; }

        [ForeignKey("IdUzytkownika")]
        public Uzytkownik Uzytkownik { get; set; }

        [ForeignKey("IdGry")]
        public Gra Gra { get; set; }
    }
}
