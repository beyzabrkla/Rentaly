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
                    query = query.Where(c => c.Category != null &&
                                             c.Category.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase));

                if (branch.HasValue && branch.Value > 0)
                    query = query.Where(c => c.BranchId == branch.Value);

                if (!string.IsNullOrEmpty(fuel))
                    query = query.Where(c => c.FuelType != null &&
                                             c.FuelType.Equals(fuel, StringComparison.OrdinalIgnoreCase));

                if (seats.HasValue && seats.Value > 0)
                {
                    if (seats.Value == 7)
                        query = query.Where(c => c.SeatCount >= 7);
                    else
                        query = query.Where(c => c.SeatCount == seats.Value);
                }

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c =>
                        (c.Brand != null && c.Brand.BrandName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                        (c.CarModel != null && c.CarModel.ModelName.Contains(search, StringComparison.OrdinalIgnoreCase)));

                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                int totalCars = query.Count();
                int totalPages = (int)Math.Ceiling(totalCars / (double)PAGE_SIZE);
                page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

                var pagedCars = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalCars = totalCars;
                ViewBag.Branches = await _branchService.TGetListAsync();

                return View(pagedCars);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araçlar yüklenirken bir hata oluştu.";
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 0;
                ViewBag.TotalCars = 0;
                ViewBag.Branches = new List<Branch>();
                return View(new List<Car>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CarDetail(int id)
        {
            try
            {
                var car = await _carService.TGetByIdAsync(id);
                if (car == null)
                    return NotFound("Araç bulunamadı.");

                if (!car.IsActive)
                    return NotFound("Bu araç şu anda mevcut değildir.");

                return View(car);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç detayları yüklenirken hata oluştu.";
                return RedirectToAction("CarList");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCarsByCategory(int categoryId)
        {
            try
            {
                var cars = await _carService.GetCarsByCategoryAsync(categoryId);
                return Json(new { success = true, data = cars });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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