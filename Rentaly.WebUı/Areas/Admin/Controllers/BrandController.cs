using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICarService _carService;

        public BrandController(IBrandService brandService, IUnitOfWork unitOfWork, ICarService carService)
        {
            _brandService = brandService;
            _unitOfWork = unitOfWork;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> BrandList(string search, int page = 1)
        {
            try
            {
                var allBrands = await _brandService.TGetListAsync();
                var allCars = await _carService.TGetListAsync();

                var filteredBrands = allBrands.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    filteredBrands = filteredBrands.Where(x => x.BrandName.ToLower().Contains(search));
                }

                ViewBag.TotalCount = allBrands.Count;

                int pageSize = 8;
                var brandList = filteredBrands.ToList();
                int totalItems = brandList.Count;

                var pagedData = brandList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                ViewBag.CarCounts = allBrands.ToDictionary(
                     b => b.BrandId,
                     b => allCars.Count(c => c.BrandId == b.BrandId)
                 );

                return View(pagedData);
            }
            catch (Exception)
            {
                TempData["Error"] = "Markalar yüklenirken bir hata oluştu.";
                ViewBag.CarCounts = new Dictionary<int, int>();
                return View(new List<Brand>());
            }
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            var validator = new BrandValidator();
            var result = validator.Validate(brand);

            if (!result.IsValid)
            {
                TempData["Error"] = string.Join("<br/>", result.Errors.Select(x => x.ErrorMessage));
                return RedirectToAction("BrandList");
            }

            try
            {
                await _brandService.TInsertAsync(brand);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Marka başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veritabanına kaydedilirken hata: " + ex.Message;
            }
            return RedirectToAction("BrandList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrand(Brand brand)
        {
            var validator = new BrandValidator();
            var result = validator.Validate(brand);

            if (!result.IsValid)
            {
                TempData["Error"] = string.Join("<br/>", result.Errors.Select(x => x.ErrorMessage));
                return RedirectToAction("BrandList");
            }

            try
            {
                await _brandService.TUpdateAsync(brand);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Marka başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme hatası: " + ex.Message;
            }
            return RedirectToAction("BrandList");
        }

    }
}
