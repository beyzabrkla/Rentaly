using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using System.Threading.Tasks;

namespace Rentaly.WebUI.ViewComponents
{
    public class _HomeCarCategoryComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _HomeCarCategoryComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.TGetListAsync();
            return View(values);
        }
    }
}