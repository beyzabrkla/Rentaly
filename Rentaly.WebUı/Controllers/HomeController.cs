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
                var cars = await _carService.GetAvailableWithDetailsAsync();
                var allBranches = await _branchService.TGetListAsync();

                // Sadece AKTİF olanları al
                var activeBranches = allBranches.Where(x => x.IsActive).ToList();

                ViewBag.TotalCars = cars.Count;
                ViewBag.TotalBranches = activeBranches.Count; // Burayı da aktif sayısına göre güncelle
                ViewBag.Branches = activeBranches; // View'a sadece bunları gönder
                ViewBag.Categories = await _categoryService.TGetListAsync();

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
            string carType,
            int pickupLocationId,      // Şube ID 
            int dropoffLocationId,     // Şube ID 
            string pickUpDate,         
            string pickUpTime,
            string dropOffDate,        
            string dropOffTime)
        {
            try
            {
                //Tarih ve Saatleri birleştirerek tam DateTime objeleri oluşturuyoruz
                if (!DateTime.TryParse($"{pickUpDate} {pickUpTime}", out DateTime start) ||
                    !DateTime.TryParse($"{dropOffDate} {dropOffTime}", out DateTime end))
                {
                    TempData["Error"] = "Lütfen geçerli bir tarih ve saat seçiniz.";
                    return RedirectToAction("Index");
                }

                // Bu metod, seçilen tarihlerde 'Onaylı' veya 'Beklemede' olan araçları otomatik eler.
                var availableCars = await _carService.GetAvailableCarsByDateAsync(start, end, pickupLocationId);

                //Eğer kategori (Sedan, SUV vb.) seçildiyse onu da filtrele
                if (!string.IsNullOrEmpty(carType))
                {
                    availableCars = availableCars
                        .Where(c => c.Category != null &&
                                    c.Category.CategoryName.Equals(carType, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                //Seçilen bu tarihleri Rental sayfasına taşımak için TempData'ya atıyoruz.
                TempData["SelectedPickupDate"] = start.ToString("yyyy-MM-ddTHH:mm");
                TempData["SelectedReturnDate"] = end.ToString("yyyy-MM-ddTHH:mm");
                TempData["SelectedPickupBranch"] = pickupLocationId;
                TempData["SelectedReturnBranch"] = dropoffLocationId;

                return View("../Car/CarList", availableCars);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Araç arama hatası: {ex.Message}");
                TempData["Error"] = "Araç aranırken teknik bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }
        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}