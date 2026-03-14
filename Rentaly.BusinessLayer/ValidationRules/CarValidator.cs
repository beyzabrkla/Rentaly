using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(x => x.PlateNumber)
                .NotEmpty().WithMessage("Plaka boş geçilemez")
                .MaximumLength(10).WithMessage("Plaka en fazla 10 karakter olabilir");

            RuleFor(x => x.VIN)
                .NotEmpty().WithMessage("Şasi numarası boş geçilemez")
                .Length(17).WithMessage("Şasi numarası 17 karakter olmalıdır");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Marka seçimi yapılmalıdır");

            RuleFor(x => x.CarModelId)
                .GreaterThan(0).WithMessage("Model seçimi yapılmalıdır");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Kategori seçimi yapılmalıdır");

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Şube seçimi yapılmalıdır");

            RuleFor(x => x.Year)
                .InclusiveBetween(1990, DateTime.Now.Year)
                .WithMessage("Araç yılı geçerli bir değer olmalıdır");

            RuleFor(x => x.Kilometer)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Kilometre negatif olamaz");

            RuleFor(x => x.DailyPrice)
                .GreaterThan(0)
                .WithMessage("Günlük fiyat 0'dan büyük olmalıdır");

            RuleFor(x => x.DepositAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Depozito negatif olamaz");

            RuleFor(x => x.CoverImageUrl)
                .NotEmpty().WithMessage("Araç görseli boş olamaz");

            RuleFor(x => x.SeatCount)
                .InclusiveBetween(1, 12)
                .WithMessage("Koltuk sayısı 1 ile 12 arasında olmalıdır");

            RuleFor(x => x.LuggageCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Bagaj sayısı negatif olamaz");

            RuleFor(x => x.FuelType)
                .NotEmpty().WithMessage("Yakıt tipi boş geçilemez");

            RuleFor(x => x.Transmission)
                .NotEmpty().WithMessage("Şanzıman tipi (Manuel/Otomatik) seçilmelidir.");
        }
    }
}
