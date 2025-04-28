using FluentValidation;
using WypozyczalniaGier.ViewModels;

public class WypozyczenieViewModelValidator : AbstractValidator<WypozyczenieViewModel>
{
    public WypozyczenieViewModelValidator()
    {
        RuleFor(x => x.IdUzytkownika)
            .GreaterThan(0).WithMessage("Użytkownik musi zostać wybrany.");

        RuleFor(x => x.IdGry)
            .GreaterThan(0).WithMessage("Gra musi zostać wybrana.");

        RuleFor(x => x.DataWypozyczenia)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data wypożyczenia nie może być w przyszłości.");

        RuleFor(x => x.DataZwrotu)
            .GreaterThan(x => x.DataWypozyczenia).WithMessage("Data zwrotu musi być późniejsza niż data wypożyczenia.");

        RuleFor(x => x.Kara)
            .GreaterThanOrEqualTo(0).WithMessage("Kara nie może być ujemna.");

        RuleFor(x => x.Koszt)
            .GreaterThanOrEqualTo(0).WithMessage("Koszt nie może być ujemny.");
    }
}
