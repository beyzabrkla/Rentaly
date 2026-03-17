using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ICategoryService categoryService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                var values = await _categoryService.TGetListAsync();
                return View(values);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategoriler yüklenirken bir hata oluştu.";
                return View(new List<Category>());
            }
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                await _categoryService.TInsertAsync(category);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(category);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            try
            {
                var value = await _categoryService.TGetByIdAsync(id);
                if (value == null)
                    return NotFound("Kategori bulunamadı.");

                return View(value);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategori yüklenirken hata oluştu.";
                return RedirectToAction("CategoryList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                await _categoryService.TUpdateAsync(category);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryService.TGetByIdAsync(id);
                if (category == null)
                    return Json(new { success = false, message = "Kategori bulunamadı." });

                await _categoryService.TDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, message = "Kategori silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }
    }
}