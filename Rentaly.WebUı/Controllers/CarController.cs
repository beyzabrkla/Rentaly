using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;
        private const int PAGE_SIZE = 9;

        public CarController(ICarService carService, IBranchService branchService)
        {
            _carService = carService;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> CarList(
                    int page = 1,
                    string? category = null,
                    int? branch = null,
                    string? fuel = null,
                    int? seats = null,
                    string? search = null,
                    decimal? minPrice = null,
                    decimal? maxPrice = null)
        {
            try
            {
                var data = await _carService.GetAvailableWithDetailsAsync();
                var query = data.AsQueryable();

                if (!string.IsNullOrEmpty(category) && category != "0")
                {
                    query = query.Where(c => c.Category != null &&
                          c.Category.CategoryName.Trim().Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (branch.HasValue && branch.Value > 0)
                    query = query.Where(c => c.BranchId == branch.Value);

                if (!string.IsNullOrEmpty(fuel))
                    query = query.Where(c => c.FuelType != null &&
                          c.FuelType.Equals(fuel, StringComparison.OrdinalIgnoreCase));

                if (seats.HasValue && seats.Value > 0)
                    query = (seats.Value >= 7) ? query.Where(c => c.SeatCount >= 7) : query.Where(c => c.SeatCount == seats.Value);

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(c =>
                        (c.Brand != null && c.Brand.BrandName.ToLower().Contains(search)) ||
                        (c.CarModel != null && c.CarModel.ModelName.ToLower().Contains(search)));
                }

                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                //Sayfalama Hesaplamaları
                int totalCars = query.Count();
                int totalPages = (int)Math.Ceiling(totalCars / (double)PAGE_SIZE);

                // Sayfa sınırlarını kontrol et
                page = page < 1 ? 1 : (totalPages > 0 && page > totalPages ? totalPages : page);

                var pagedCars = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                //View'a gidecek ek veriler
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalCars = totalCars;
                ViewBag.Branches = await _branchService.TGetListAsync();

                //Aktif filtreleri View'da tutmak için (URL'leri bozmamak adına)
                ViewBag.SelectedCategory = category;
                ViewBag.SelectedBranch = branch;

                return View(pagedCars);
            }
            catch (Exception ex)
            {
                // Hata günlüğü tutulabilir: _logger.LogError(ex, "CarList error");
                TempData["Error"] = "Araçlar yüklenirken teknik bir sorun oluştu.";
                return View(new List<Car>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CarDetail(int id)
        {
            if (id <= 0) return RedirectToAction("CarList");

            var car = await _carService.TGetByIdAsync(id);

            if (car == null || !car.IsActive)
            {
                TempData["Warning"] = "Aradığınız araç şu anda kiralamaya kapalıdır.";
                return RedirectToAction("CarList");
            }

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> GetCarsByCategory(int categoryId)
        {
            var cars = await _carService.GetCarsByCategoryAsync(categoryId);
            return Json(new { success = true, count = cars.Count, data = cars });
        }

        [HttpGet]
        public async Task<JsonResult> GetCarsByBrand(int brandId)
        {
            try
            {
                var cars = await _carService.GetCarsByBrandAsync(brandId);
                return Json(new { success = true, data = cars });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCarsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var cars = await _carService.GetCarsByPriceRangeAsync(minPrice, maxPrice);
                return Json(new { success = true, data = cars });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}