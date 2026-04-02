using FluentValidation;
using Rentaly.DTOLayer.RentalDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.RentalValidator
{
    public class CreateRentalValidator : AbstractValidator<CreateRentalDTO>
    {
        public CreateRentalValidator()
        {
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Araç seçimi zorunludur.");
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Müşteri seçimi zorunludur.");

            RuleFor(x => x.PickupDate)
                .NotEmpty().WithMessage("Alış tarihi boş bırakılamaz.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Alış tarihi geçmiş bir tarih olamaz.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty().WithMessage("İade tarihi boş bırakılamaz.")
                .GreaterThan(x => x.PickupDate).WithMessage("İade tarihi, alış tarihinden sonra olmalıdır.");

            RuleFor(x => x.PickupBranchId).GreaterThan(0).WithMessage("Alış şubesi seçilmelidir.");
            RuleFor(x => x.ReturnBranchId).GreaterThan(0).WithMessage("İade şubesi seçilmelidir.");

            RuleFor(x => x.Status).NotEmpty().WithMessage("Kiralama durumu belirtilmelidir.");

        }
    }
}
