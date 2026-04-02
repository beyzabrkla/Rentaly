using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules.CategoryValidator;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.CategoryDTOs;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                var values = await _categoryService.TGetListAsync();
                var categoryDtoList = _mapper.Map<List<ResultCategoryDTO>>(values);
                return View(values);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategoriler yüklenirken bir hata oluştu.";
                return View(new List<ResultCategoryDTO>());
            }
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                var validator = new CreateCategoryValidator();
                var result = await validator.ValidateAsync(createCategoryDTO);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    return View(createCategoryDTO);
                }

                var category = _mapper.Map<Category>(createCategoryDTO);

                await _categoryService.TInsertAsync(category);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(createCategoryDTO);
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

                var dto = _mapper.Map<UpdateCategoryDTO>(value);

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategori yüklenirken hata oluştu.";
                return RedirectToAction("CategoryList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var validator = new UpdateCategoryValidator();
                var result = await validator.ValidateAsync(updateCategoryDTO);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    return View(updateCategoryDTO);
                }

                var category = await _categoryService.TGetByIdAsync(updateCategoryDTO.CategoryId);
                if (category == null) return NotFound();
                _mapper.Map(updateCategoryDTO, category);

                await _categoryService.TUpdateAsync(category);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("CategoryList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return View(updateCategoryDTO);
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