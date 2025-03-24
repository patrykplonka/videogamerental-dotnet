using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaGier.Models
{
    public class Uzytkownik
    {
        [Key]
        public int IdUzytkownika { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
    }
}