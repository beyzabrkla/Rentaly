using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities; 

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CategoryList()
        {
            var values = await _categoryService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!string.IsNullOrEmpty(category.CategoryName))
            {
                await _categoryService.TInsertAsync(category);
            }
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null) return NotFound();

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (category.CategoryId > 0 && !string.IsNullOrEmpty(category.CategoryName))
            {
                await _categoryService.TUpdateAsync(category);
            }
            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.TDeleteAsync(id);

            return RedirectToAction("CategoryList");
        }
    }
}
