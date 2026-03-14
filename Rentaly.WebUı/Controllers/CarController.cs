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
            int page = 1, string category = null, int? branch = null,
            string fuel = null, int? seats = null, string search = null, // search parametresini ekledik
            decimal? minPrice = null, decimal? maxPrice = null)
        {
            try
            {
                // Veriyi çek
                var data = await _carService.GetAvailableCarsAsync();
                var query = data.AsQueryable();


                // Kategori Filtresi
                if (!string.IsNullOrEmpty(category) && category != "0")
                {
                    // Her iki tarafı da küçük harfe çevirerek eşleştirme yapıyoruz
                    query = query.Where(c => c.Category != null &&
                                             c.Category.CategoryName.ToLower() == category.ToLower());
                }
                
                // Şube Filtresi
                if (branch.HasValue && branch.Value > 0)
                    query = query.Where(c => c.BranchId == branch.Value);

                // Yakıt Filtresi
                if (!string.IsNullOrEmpty(fuel))
                    query = query.Where(c => c.FuelType == fuel);

                // Koltuk Sayısı Filtresi (EKSİKTİ, EKLENDİ)
                if (seats.HasValue && seats.Value > 0)
                    query = query.Where(c => c.SeatCount == seats.Value);

                // Arama Filtresi (Marka veya Model içeriyorsa)
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c => (c.Brand != null && c.Brand.BrandName.Contains(search)) ||
                                             (c.CarModel != null && c.CarModel.ModelName.Contains(search)));

                // Fiyat Filtreleri
                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                //Sayfalama 
                int totalCars = query.Count();
                int totalPages = (int)Math.Ceiling(totalCars / (double)PAGE_SIZE);

                // Sayfa doğrulama
                page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

                // Skip ve Take
                var pagedCars = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                // Viewbag atamaları
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalCars = totalCars;
                ViewBag.Branches = await _branchService.TGetListAsync();

                return View(pagedCars);
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir (ex)
                TempData["Error"] = "Araçlar yüklenirken bir hata oluştu.";
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
                {
                    return NotFound("Araç bulunamadı.");
                }

                if (!car.IsActive)
                {
                    return NotFound("Bu araç şu anda mevcut değildir.");
                }
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