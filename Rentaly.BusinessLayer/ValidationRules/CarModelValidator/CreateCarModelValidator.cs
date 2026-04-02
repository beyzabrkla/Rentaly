
using FluentValidation;
using Rentaly.DTOLayer.CarModelDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CarModelValidator
{
    public class CreateCarModelValidator : AbstractValidator<CreateCarModelDTO>
    {
        public CreateCarModelValidator()
        {
            RuleFor(x => x.ModelName)
                .NotEmpty().WithMessage("Model adı boş geçilemez.")
                .MaximumLength(50).WithMessage("Model adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Lütfen geçerli bir marka seçiniz.");

        }
    }
}
