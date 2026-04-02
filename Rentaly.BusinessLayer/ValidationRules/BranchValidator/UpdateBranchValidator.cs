using FluentValidation;
using Rentaly.DTOLayer.BranchDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.BranchValidator
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchDTO>
    {
        public UpdateBranchValidator()
        {
            Include (new CreateBranchValidator());

            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Güncellenecek Şube Bulunamadı");
        }
    }
}
