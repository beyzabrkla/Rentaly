using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class BrandValidator :AbstractValidator<Brand>
    {
        public BrandValidator()
        {
            RuleFor(x => x.BrandName)
                .NotEmpty().WithMessage("Marka adı boş geçilemez");

            RuleFor(x => x.BrandName)
                .MinimumLength(2).WithMessage("Marka adı en az 2 karakter olmalıdır");

            RuleFor(x => x.BrandName)
                .MaximumLength(30).WithMessage("Marka adı en fazla 30 karakter olabilir");

            RuleFor(x => x.BrandName)
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$")
                .WithMessage("Marka adı sadece harf içermelidir");

            RuleFor(x => x.BrandName)
                .Must(StartWithUpperLetter)
                .WithMessage("Marka adı büyük harfle başlamalıdır");

            RuleFor(x => x.CoverImageUrl)
                .NotNull().WithMessage("Marka görseli null olamaz");

            RuleFor(x => x.CoverImageUrl)
                .Must(BeAValidUrl)
                .WithMessage("Geçerli bir görsel URL giriniz");
        }

        private bool StartWithUpperLetter(string brandName)
        {
            if (string.IsNullOrEmpty(brandName))
                return false;

            return char.IsUpper(brandName[0]);
        }

        private bool BeAValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

    }
}
