using FluentValidation;
using WypozyczalniaGier.ViewModels;

public class UzytkownikViewModelValidator : AbstractValidator<UzytkownikViewModel>
{
    public UzytkownikViewModelValidator()
    {
        RuleFor(x => x.Imie)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(50).WithMessage("Imię nie może mieć więcej niż 50 znaków.");

        RuleFor(x => x.Nazwisko)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(50).WithMessage("Nazwisko nie może mieć więcej niż 50 znaków.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy adres email.");
    }
}
