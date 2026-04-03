
using FluentValidation;
using Rentaly.DTOLayer.CarDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CarValidator
{
    public class UpdateCarValidator : AbstractValidator<UpdateCarDTO>
    {
        public UpdateCarValidator()
        {
            Include(new CreateCarValidator());

            RuleFor(x => x.CarId).NotEmpty().WithMessage("Güncellenecek Araç Bulunamadı.");
            
            RuleFor(x => x.CoverImageUrl).NotNull().When(x => false);
        }
    }
}
