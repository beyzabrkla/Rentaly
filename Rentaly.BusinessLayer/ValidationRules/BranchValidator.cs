using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class BranchValidator : AbstractValidator<Branch>
    {
        public BranchValidator()
        {
            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Şube adı boş geçilemez.")
                .MaximumLength(100).WithMessage("Şube adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir alanı boş geçilemez.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("Lütfen daha detaylı bir adres giriniz.");
        }
    }
}
