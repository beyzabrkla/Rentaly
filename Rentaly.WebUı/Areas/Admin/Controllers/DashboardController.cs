using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IBrandService _brandService;
        private readonly ICarService _carService;
        private readonly ICarModelService _carModelService;
        private readonly ICategoryService _categoryService;

        public DashboardController(IBranchService branchService, IBrandService brandService, ICarService carService, ICarModelService carModelService, ICategoryService categoryService)
        {
            _branchService = branchService;
            _brandService = brandService;
            _carService = carService;
            _carModelService = carModelService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var allCars = await _carService.TGetListAsync();

            // --- 1. DİNAMİK GRAFİK VERİSİ (AYLIK ANALİZ) ---
            // Son 6 ayın verisini gruplayıp aylara göre sayılarını alıyoruz
            var monthlyData = allCars
                .Where(x => x.CreatedDate >= DateTime.Now.AddMonths(-5)) // Son 6 ayın verisi
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month })
                .Select(group => new {
                    // Ay ismini al (Örn: Ocak, Şubat)
                    MonthName = new DateTime(group.Key.Year, group.Key.Month, 1).ToString("MMMM"),
                    Count = group.Count(),
                    SortValue = group.Key.Year * 100 + group.Key.Month // Doğru kronolojik sıralama için
                })
                .OrderBy(x => x.SortValue)
                .ToList();

            ViewBag.ChartLabels = monthlyData.Select(x => x.MonthName).ToList();
            ViewBag.ChartData = monthlyData.Select(x => x.Count).ToList();

            // --- 2. SON HAREKETLER (YENİDEN ESKİYE) ---
            // En son eklenen 5 aracı listeliyoruz
            ViewBag.RecentCars = allCars.OrderByDescending(x => x.CreatedDate).Take(5).ToList();

            // --- 3. DİĞER ÖZET VERİLER ---
            ViewBag.BranchCount = (await _branchService.TGetListAsync()).Count;
            ViewBag.BrandCount = (await _brandService.TGetListAsync()).Count;
            ViewBag.CarCount = allCars.Count;
            ViewBag.ModelCount = (await _carModelService.TGetListAsync()).Count;
            ViewBag.CategoryCount = (await _categoryService.TGetListAsync()).Count;

            // Toplam araç içindeki IsActive (Aktif) olanların oranını hesapla
            double efficiency = allCars.Count > 0
                ? (double)allCars.Count(x => x.IsActive) / allCars.Count * 100
                : 0;

            ViewBag.Efficiency = Math.Round(efficiency, 0);

            return View();
        }
    }
}
