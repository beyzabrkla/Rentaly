using FluentValidation;
using Rentaly.DTOLayer.RentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules.RentalValidator
{
    public class CreateRentalValidator : AbstractValidator<CreateRentalDTO>
    {
        public CreateRentalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad alanı boş geçilemez.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş geçilemez.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
            RuleFor(x => x.Phone).Matches(@"^\d{10,11}$").WithMessage("Geçerli bir telefon numarası giriniz.");
            RuleFor(x => x.IdentityNumber).Length(11).WithMessage("TC Kimlik No 11 hane olmalıdır.");
            RuleFor(x => x.ReturnBranchId).NotEmpty().WithMessage("Lütfen dönüş şubesi seçiniz.");
            RuleFor(x => x.PickupDate).GreaterThan(DateTime.Now).WithMessage("Alış tarihi geçmiş bir tarih olamaz.");
            RuleFor(x => x.ReturnDate).GreaterThan(x => x.PickupDate).WithMessage("Dönüş tarihi alış tarihinden sonra olmalıdır.");
        }
    }
}
