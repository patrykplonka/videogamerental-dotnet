namespace WypozyczalniaGier.ViewModels
{
    public class WypozyczenieViewModel
    {
        public int IdWypozyczenia { get; set; }
        public int IdUzytkownika { get; set; }
        public string UzytkownikNazwa { get; set; }

        public int IdGry { get; set; }
        public string GraTytul { get; set; }

        public DateTime DataWypozyczenia { get; set; }
        public DateTime DataZwrotu { get; set; }
        public DateTime? DataZwrotuRzeczywista { get; set; }
        public decimal Kara { get; set; }
        public decimal Koszt { get; set; }
    }
}
