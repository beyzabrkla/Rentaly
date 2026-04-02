using FluentValidation;
using Rentaly.DTOLayer.BranchDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.BranchValidator
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchDTO>
    {
        public CreateBranchValidator()
        {
            RuleFor(x => x.BranchName).NotEmpty().WithMessage("Şube adı boş geçilemez.")
                                       .MaximumLength(100).WithMessage("Şube adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir alanı boş geçilemez.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("Lütfen daha detaylı bir adres giriniz.");

        }
    }
}
