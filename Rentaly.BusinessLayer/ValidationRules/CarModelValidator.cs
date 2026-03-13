using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CarModelValidator : AbstractValidator<CarModel>
    {
        public CarModelValidator()
        {
            RuleFor(x => x.ModelName)
                            .NotEmpty().WithMessage("Model adı boş geçilemez.")
                            .MaximumLength(50).WithMessage("Model adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Lütfen geçerli bir marka seçiniz.");
        }
    }
}
