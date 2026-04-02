using FluentValidation;
using Rentaly.DTOLayer.CustomerDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CustomerValidator
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDTO>
    {
        public UpdateCustomerValidator()
        {
            Include(new CreateCustomerValidator());
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Güncellenecek Müşteri Bulunamadı.");
        }
    }
}
