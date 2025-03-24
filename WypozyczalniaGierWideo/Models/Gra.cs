using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaGier.Models
{
    public class Gra
    {
        [Key]
        public int IdGry { get; set; }

        [Required]
        [MaxLength(100)]
        public string Tytul { get; set; }

        [Required]
        [MaxLength(50)]
        public string Gatunek { get; set; }

        [Required]
        [MaxLength(50)]
        public string Platforma { get; set; }

        [Required]
        public decimal CenaZaDzien { get; set; }

        public ICollection<Wypozyczenie> Wypozyczenia { get; set; }
    }
}
