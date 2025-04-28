using FluentValidation;
using WypozyczalniaGier.ViewModels;

public class GraViewModelValidator : AbstractValidator<GraViewModel>
{
    public GraViewModelValidator()
    {
        RuleFor(x => x.Tytul)
            .NotEmpty().WithMessage("Tytuł jest wymagany.")
            .MaximumLength(100).WithMessage("Tytuł nie może mieć więcej niż 100 znaków.");

        RuleFor(x => x.Gatunek)
            .NotEmpty().WithMessage("Gatunek jest wymagany.");

        RuleFor(x => x.Platforma)
            .NotEmpty().WithMessage("Platforma jest wymagana.");

        RuleFor(x => x.CenaZaDzien)
            .GreaterThan(0).WithMessage("Cena za dzień musi być większa od zera.");
    }
}
