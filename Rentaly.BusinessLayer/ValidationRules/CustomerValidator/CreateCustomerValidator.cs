using FluentValidation;
using Rentaly.DTOLayer.CustomerDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CustomerValidator
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDTO>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.")
                        .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.")
                        .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .Matches(@"^(\+90|0)?5\d{9}$")
                .WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("Kimlik numarası boş bırakılamaz.")
                .Length(11).WithMessage("Kimlik numarası 11 haneli olmalıdır.")
                .Matches(@"^\d{11}$")
                .WithMessage("Kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.DrivingLicenseNumber)
                .NotEmpty().WithMessage("Ehliyet numarası boş bırakılamaz.")
                .MinimumLength(5).WithMessage("Ehliyet numarası en az 5 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Ehliyet numarası en fazla 20 karakter olabilir.");

            RuleFor(x => x.DrivingLicenseDate)
                .NotEmpty().WithMessage("Ehliyet tarihi boş bırakılamaz.")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Ehliyet tarihi gelecekte olamaz.");

        }
    }
}
