using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUı.Models;
using System.Diagnostics;

namespace Rentaly.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            ILogger<HomeController> logger,
            ICarService carService,
            IBranchService branchService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _carService = carService;
            _branchService = branchService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cars       = await _carService.GetAvailableWithDetailsAsync();
                var branches   = await _branchService.TGetListAsync();
                var categories = await _categoryService.TGetListAsync();

                ViewBag.TotalCars      = cars.Count;
                ViewBag.TotalBranches  = branches.Count;
                ViewBag.Branches       = branches;
                ViewBag.Categories     = categories;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ana sayfa yüklenirken hata: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchCars(
            string carType, string pickupLocation, string dropoffLocation,
            DateTime pickUpDate, string pickUpTime,
            string collectionDate, string collectionTime)
        {
            try
            {
                var cars = await _carService.GetAvailableWithDetailsAsync();

                if (!string.IsNullOrEmpty(carType))
                {
                    cars = cars
                        .Where(c => c.Category != null &&
                                    c.Category.CategoryName.Contains(carType, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                TempData["SearchParams"] = $"Tür: {carType}, Alış: {pickupLocation}, Dönüş: {dropoffLocation}";
                return RedirectToAction("CarList", "Car");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Araç arama hatası: {ex.Message}");
                TempData["Error"] = "Araç aranırken hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}