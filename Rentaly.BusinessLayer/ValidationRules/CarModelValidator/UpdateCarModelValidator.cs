
using FluentValidation;
using Rentaly.DTOLayer.CarModelDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CarModelValidator
{
    public class UpdateCarModelValidator : AbstractValidator<UpdateCarModelDTO>
    {
        public UpdateCarModelValidator()
        {
            Include(new CreateCarModelValidator());

            RuleFor(x => x.CarModelId)
                .GreaterThan(0).WithMessage("Güncellenecek Model Bulunamadı.");
        }
    }
}
