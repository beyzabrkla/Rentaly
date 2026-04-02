using FluentValidation;
using Rentaly.DTOLayer.RentalDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.RentalValidator
{
    public class UpdateRentalValidator : AbstractValidator<UpdateRentalDTO>
    {
        public UpdateRentalValidator()
        {
            Include(new CreateRentalValidator());

            RuleFor(x => x.RentalId).NotEmpty().WithMessage("Güncellenecek Rental Id si Bulunamadı.");
        }
    }
}
