using FluentValidation;
using Rentaly.DTOLayer.BrandDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.BrandValidator
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandDTO>
    {
        public UpdateBrandValidator()
        {
            Include(new CreateBrandValidator());

            RuleFor(x => x.BrandId).NotEmpty().WithMessage("Güncellenecek Marka Bulunamadı.");

        }
    }
}

