using FluentValidation;
using Rentaly.DTOLayer.BrandDTOs;

public class CreateBrandValidator : AbstractValidator<CreateBrandDTO>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.BrandName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Marka adı boş geçilemez")
            .MinimumLength(2)
                .When(x => !string.IsNullOrWhiteSpace(x.BrandName))
                .WithMessage("Marka adı en az 2 karakter olmalıdır")
            .MaximumLength(30)
                .When(x => !string.IsNullOrWhiteSpace(x.BrandName))
                .WithMessage("Marka adı en fazla 30 karakter olabilir")
            .Must(StartWithUpperLetter)
                .When(x => !string.IsNullOrEmpty(x.BrandName))
                .WithMessage("Marka adı büyük harfle başlamalıdır");

        RuleFor(x => x.CoverImageUrl)
            .Must(BeAValidUrl)
            .When(x => !string.IsNullOrEmpty(x.CoverImageUrl))
            .WithMessage("Geçerli bir görsel URL giriniz");
    }

    private bool StartWithUpperLetter(string brandName)
    {
        if (string.IsNullOrEmpty(brandName)) return false;
        return char.IsUpper(brandName[0]);
    }

    private bool BeAValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}