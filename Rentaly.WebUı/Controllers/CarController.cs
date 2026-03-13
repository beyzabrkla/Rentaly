using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // Araç Listesi Sayfası - Müsait araçları göster
        [HttpGet]
        public async Task<IActionResult> CarList()
        {
            try
            {
                // Müsait ve aktif araçları getir
                var cars = await _carService.GetAvailableCarsAsync();
                return View(cars);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Araçlar yüklenirken hata oluştu: {ex.Message}";
                return View(new List<Car>());
            }
        }

        // Araç Detayı Sayfası
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

                // Sadece aktif araçları göster
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


        // Kategoriye göre araçları getir (AJAX)
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

        // Markaya göre araçları getir (AJAX)
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

        // Fiyat aralığına göre araçları getir (AJAX)
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