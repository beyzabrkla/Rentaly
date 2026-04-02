using FluentValidation;
using Rentaly.DTOLayer.CategoryDTOs;

namespace Rentaly.BusinessLayer.ValidationRules.CategoryValidator
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            Include(new CreateCategoryValidator());
             RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Güncellenecek Kategori Bulunamadı.");
        }
    }
}
