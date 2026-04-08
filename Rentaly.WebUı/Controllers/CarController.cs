using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DTOLayer.CarDTOs;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private const int PAGE_SIZE = 9;

        public CarController(ICarService carService, IBranchService branchService, IMapper mapper, ICategoryService categoryService)
        {
            _carService = carService;
            _branchService = branchService;
            _mapper = mapper;
            _categoryService = categoryService;
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
                            decimal? maxPrice = null,
                            DateTime? pickupDate = null,
                            DateTime? dropOffDate = null)
        {
            try
            {
                IEnumerable<Car> data;

                if (pickupDate.HasValue && dropOffDate.HasValue)
                {
                    // Tarih seçiliyse müsait araçları getir (Şube seçili değilse 0 gönderiyoruz ki tümünde arasın)
                    data = await _carService.GetAvailableCarsByDateAsync(pickupDate.Value, dropOffDate.Value, branch ?? 0);
                }
                else
                {
                    // Tarih seçili değilse tüm araçları detaylarıyla getir
                    data = await _carService.GetAvailableWithDetailsAsync();
                }

                var query = data.AsQueryable();

                // Şube Filtresi: Eğer tarih seçili DEĞİLSE manuel olarak filtrele. 
                // (Tarih seçiliyse zaten GetAvailableCarsByDateAsync şube bazlı getirmiştir)
                if (!pickupDate.HasValue && branch.HasValue && branch.Value > 0)
                {
                    query = query.Where(c => c.BranchId == branch.Value);
                }

                // Kategori Filtresi
                if (!string.IsNullOrEmpty(category) && category != "0")
                {
                    query = query.Where(c => c.Category != null &&
                          c.Category.CategoryName.Trim().Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                // Yakıt Tipi Filtresi
                if (!string.IsNullOrEmpty(fuel))
                {
                    query = query.Where(c => c.FuelType != null && c.FuelType.Equals(fuel, StringComparison.OrdinalIgnoreCase));
                }

                // Koltuk Sayısı Filtresi
                if (seats.HasValue && seats.Value > 0)
                {
                    query = (seats.Value >= 7) ? query.Where(c => c.SeatCount >= 7) : query.Where(c => c.SeatCount == seats.Value);
                }

                // Arama (Marka/Model) Filtresi
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.ToLower();
                    query = query.Where(c => (c.Brand != null && c.Brand.BrandName.ToLower().Contains(searchLower)) ||
                                             (c.CarModel != null && c.CarModel.ModelName.ToLower().Contains(searchLower)));
                }

                // Fiyat Filtresi
                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                //Sayfalama Mantığı
                int totalCars = query.Count();
                int totalPages = (int)Math.Ceiling(totalCars / (double)PAGE_SIZE);

                // Geçersiz sayfa isteklerini engelle
                page = page < 1 ? 1 : page;

                var pagedCars = query.OrderByDescending(c => c.CarId)
                                     .Skip((page - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE)
                                     .ToList();

                // Sadece AKTİF şubeleri çekiyoruz
                ViewBag.Branches = (await _branchService.TGetListAsync()).Where(b => b.IsActive).ToList();
                ViewBag.Categories = await _categoryService.TGetListAsync();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalCars = totalCars;

                // UI'da filtrelerin seçili kalması için değerleri geri gönderiyoruz
                ViewBag.SelectedBranch = branch;
                ViewBag.SelectedCategory = category;
                ViewBag.SelectedFuel = fuel;
                ViewBag.SelectedSeats = seats;

                return View(_mapper.Map<List<ResultCarDTO>>(pagedCars));
            }
            catch (Exception)
            {
                // Hata durumunda boş liste döner
                return View(new List<ResultCarDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CarDetail(int id)
        {
            var car = await _carService.GetCarByIdWithDetailsAsync(id);
            if (car == null) return RedirectToAction("CarList");

            var mappedCar = _mapper.Map<GetCarByIdDTO>(car);
            var branches = await _branchService.TGetListAsync();
            ViewBag.Branches = branches.Where(x => x.IsActive).ToList();

            if (car.CarImages != null && car.CarImages.Any())
            {
                mappedCar.CarImages = car.CarImages.Select(x => x.CoverImageUrl).ToList();
            }

            return View(mappedCar);
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