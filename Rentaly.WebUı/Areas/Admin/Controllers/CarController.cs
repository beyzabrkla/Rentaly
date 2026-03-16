using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly ICarImageService _carImageService;
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;
        private readonly IUnitOfWork _unitOfWork;
        private const int PAGE_SIZE = 9;

        public CarController(
            ICarService carService,
            IBrandService brandService,
            ICarModelService carModelService,
            ICategoryService categoryService,
            IBranchService branchService,
            IUnitOfWork unitOfWork,
            ICarImageService carImageService)
        {
            _carService = carService;
            _brandService = brandService;
            _carModelService = carModelService;
            _categoryService = categoryService;
            _branchService = branchService;
            _unitOfWork = unitOfWork;
            _carImageService = carImageService;
        }

        [HttpGet]
        public async Task<IActionResult> CarList(int page = 1, string? search = null, int? brand = null,string? fuel = null, string? status = null, string? active = null,
                    decimal? minPrice = null, decimal? maxPrice = null)
        {
            try
            {
                var allCars = await _carService.GetAllWithDetailsAsync();
                ViewBag.TotalCars = allCars.Count;
                ViewBag.AvailableCount = allCars.Count(c => c.IsAvailable);
                ViewBag.RentedCount = allCars.Count(c => !c.IsAvailable);

                var query = allCars.AsQueryable();

                // ARAMA
                if (!string.IsNullOrEmpty(search))
                    query = query.Where(c =>
                        (c.Brand != null && c.Brand.BrandName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                        (c.CarModel != null && c.CarModel.ModelName.Contains(search, StringComparison.OrdinalIgnoreCase)));

                // MARKA FİLTRESİ
                if (brand.HasValue && brand.Value > 0)
                    query = query.Where(c => c.BrandId == brand.Value);

                // YAKIT FİLTRESİ
                if (!string.IsNullOrEmpty(fuel))
                    query = query.Where(c =>
                        !string.IsNullOrEmpty(c.FuelType) &&
                        c.FuelType.Equals(fuel, StringComparison.OrdinalIgnoreCase));

                // MÜSAIT/KİRADA FİLTRESİ
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(c => status == "Müsait" ? c.IsAvailable : !c.IsAvailable);

                // AKTİF/İNAKTİF FİLTRESİ (YENİ) ← ←
                if (!string.IsNullOrEmpty(active))
                    query = query.Where(c => active == "Aktif" ? c.IsActive : !c.IsActive);

                // FİYAT ARALIĞI
                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                // PAGİNASYON
                int totalFiltered = query.Count();
                int totalPages = (int)Math.Ceiling(totalFiltered / (double)PAGE_SIZE);
                page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));
                var pagedCars = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.Brands = await _brandService.TGetListAsync();

                return View(pagedCars);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araçlar yüklenirken bir hata oluştu.";
                ViewBag.TotalCars = 0;
                ViewBag.AvailableCount = 0;
                ViewBag.RentedCount = 0;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 0;
                ViewBag.Brands = new List<Brand>();
                return View(new List<Car>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            try
            {
                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Form yüklenirken hata oluştu.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar(Car car, IFormFile? imageFile, List<IFormFile>? carImageFiles, string? imageSourceType)
        {
            try
            {
                ModelState.Remove("Brand");
                ModelState.Remove("CarModel");
                ModelState.Remove("Category");
                ModelState.Remove("Branch");
                ModelState.Remove("CarImages");

                if (!ModelState.IsValid)
                {
                    ViewBag.Brands = await _brandService.TGetListAsync();
                    ViewBag.Categories = await _categoryService.TGetListAsync();
                    ViewBag.Branches = await _branchService.TGetListAsync();
                    return View(car);
                }

                await _carService.TInsertAsync(car);
                await _unitOfWork.SaveAsync();

                // === KAPAK GÖRSELI ===
                if (imageSourceType == "file" && imageFile != null && imageFile.Length > 0)
                {
                    if (imageFile.Length <= 5 * 1024 * 1024)
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                        var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);
                        var directory = Path.GetDirectoryName(uploadPath)!;

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                            await imageFile.CopyToAsync(stream);

                        car.CoverImageUrl = $"/uploads/cars/{fileName}";
                        await _carService.TUpdateAsync(car);
                        await _unitOfWork.SaveAsync();
                    }
                }
                else if (imageSourceType == "url" && !string.IsNullOrEmpty(car.CoverImageUrl))
                {
                    await _carService.TUpdateAsync(car);
                    await _unitOfWork.SaveAsync();
                }

                // === DİĞER GÖRSELLER ===
                if (carImageFiles != null && carImageFiles.Count > 0)
                {
                    foreach (var file in carImageFiles)
                    {
                        if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
                            continue;

                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);
                        var directory = Path.GetDirectoryName(uploadPath)!;

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        var imageUrl = $"/uploads/cars/{fileName}";

                        await _carImageService.TInsertAsync(new CarImage
                        {
                            CoverImageUrl = imageUrl,
                            CarId = car.CarId
                        });
                    }

                    await _unitOfWork.SaveAsync();
                }

                TempData["Success"] = "Araç başarıyla kaydedildi.";
                return RedirectToAction("CarList");
            }
            catch (Exception ex)
            {
                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(car);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCar(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdWithDetailsAsync(id);
                if (car == null)
                    return NotFound("Araç bulunamadı.");

                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();

                if (car.BrandId > 0)
                    ViewBag.Models = await _carModelService.GetModelsByBrandAsync(car.BrandId);

                return View(car);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç yüklenirken hata oluştu.";
                return RedirectToAction("CarList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(
            int id,
            Car car,
            IFormFile? imageFile,
            List<IFormFile>? carImageFiles,
            string? imageSourceType,
            List<string>? deletedImageIds)
        {
            try
            {
                if (id != car.CarId)
                    return BadRequest("Araç ID eşleşmiyor.");

                ModelState.Remove("Brand");
                ModelState.Remove("CarModel");
                ModelState.Remove("Category");
                ModelState.Remove("Branch");
                ModelState.Remove("CarImages");

                if (!ModelState.IsValid)
                {
                    ViewBag.Brands = await _brandService.TGetListAsync();
                    ViewBag.Categories = await _categoryService.TGetListAsync();
                    ViewBag.Branches = await _branchService.TGetListAsync();
                    if (car.BrandId > 0)
                        ViewBag.Models = await _carModelService.GetModelsByBrandAsync(car.BrandId);
                    return View(car);
                }

                // === KAPAK GÖRSELI ===
                if (imageSourceType == "remove")
                {
                    if (!string.IsNullOrEmpty(car.CoverImageUrl) && car.CoverImageUrl.StartsWith("/"))
                    {
                        var oldPath = Path.Combine("wwwroot", car.CoverImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }
                    car.CoverImageUrl = null;
                }
                else if (imageSourceType == "file" && imageFile != null && imageFile.Length > 0)
                {
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Görsel 5MB\'dan büyük olamaz.");
                        ViewBag.Brands = await _brandService.TGetListAsync();
                        ViewBag.Categories = await _categoryService.TGetListAsync();
                        ViewBag.Branches = await _branchService.TGetListAsync();
                        if (car.BrandId > 0)
                            ViewBag.Models = await _carModelService.GetModelsByBrandAsync(car.BrandId);
                        return View(car);
                    }

                    // Eski kapak görseli sil
                    if (!string.IsNullOrEmpty(car.CoverImageUrl) && car.CoverImageUrl.StartsWith("/"))
                    {
                        var oldPath = Path.Combine("wwwroot", car.CoverImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                    var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);
                    var directory = Path.GetDirectoryName(uploadPath)!;

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                        await imageFile.CopyToAsync(stream);

                    car.CoverImageUrl = $"/uploads/cars/{fileName}";
                }

                // === SİLİNECEK DİĞER GÖRSELLER ===
                if (deletedImageIds != null && deletedImageIds.Count > 0)
                {
                    foreach (var imageId in deletedImageIds)
                    {
                        if (int.TryParse(imageId, out int id_parsed))
                        {
                            var img = await _carImageService.TGetByIdAsync(id_parsed);
                            if (img != null)
                            {
                                // Dosyayı sil
                                if (!string.IsNullOrEmpty(img.CoverImageUrl) && img.CoverImageUrl.StartsWith("/"))
                                {
                                    var path = Path.Combine("wwwroot", img.CoverImageUrl.TrimStart('/'));
                                    if (System.IO.File.Exists(path))
                                        System.IO.File.Delete(path);
                                }

                                // DB'den sil
                                await _carImageService.TDeleteAsync(id_parsed);
                            }
                        }
                    }
                }

                // === YENİ DİĞER GÖRSELLER ===
                if (carImageFiles != null && carImageFiles.Count > 0)
                {
                    foreach (var file in carImageFiles)
                    {
                        if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
                            continue;

                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);
                        var directory = Path.GetDirectoryName(uploadPath)!;

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        var imageUrl = $"/uploads/cars/{fileName}";

                        await _carImageService.TInsertAsync(new CarImage
                        {
                            CoverImageUrl = imageUrl,
                            CarId = car.CarId
                        });
                    }
                }

                await _carService.TUpdateAsync(car);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Araç güncellendi.";
                return RedirectToAction("CarList");
            }
            catch (Exception ex)
            {
                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();
                if (car.BrandId > 0)
                    ViewBag.Models = await _carModelService.GetModelsByBrandAsync(car.BrandId);
                TempData["Error"] = $"Hata: {ex.Message}";
                return View(car);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _carService.TGetByIdAsync(id);
                if (car == null)
                    return Json(new { success = false, message = "Araç bulunamadı." });

                // Kapak görseli sil
                if (!string.IsNullOrEmpty(car.CoverImageUrl) && car.CoverImageUrl.StartsWith("/"))
                {
                    var imagePath = Path.Combine("wwwroot", car.CoverImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                // Diğer görselleri sil
                if (car.CarImages != null && car.CarImages.Count > 0)
                {
                    foreach (var img in car.CarImages)
                    {
                        if (!string.IsNullOrEmpty(img.CoverImageUrl) && img.CoverImageUrl.StartsWith("/"))
                        {
                            var path = Path.Combine("wwwroot", img.CoverImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                        }
                    }
                }

                await _carService.TDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, message = "Araç silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetModelsByBrand(int brandId)
        {
            try
            {
                var models = await _carModelService.GetModelsByBrandAsync(brandId);
                return Json(new { success = true, data = models });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAddBrand(string brandName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(brandName))
                    return Json(new { success = false, message = "Marka adı boş olamaz." });

                var brand = new Brand { BrandName = brandName.Trim() };
                await _brandService.TInsertAsync(brand);

                return Json(new { success = true, id = brand.BrandId, name = brand.BrandName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAddModel(string modelName, int brandId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelName))
                    return Json(new { success = false, message = "Model adı boş olamaz." });

                if (brandId <= 0)
                    return Json(new { success = false, message = "Önce marka seçin." });

                var model = new CarModel { ModelName = modelName.Trim(), BrandId = brandId };
                await _carModelService.TInsertAsync(model);

                return Json(new { success = true, id = model.CarModelId, name = model.ModelName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAddCategory(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    return Json(new { success = false, message = "Kategori adı boş olamaz." });

                var category = new Category { CategoryName = categoryName.Trim() };
                await _categoryService.TInsertAsync(category);

                return Json(new { success = true, id = category.CategoryId, name = category.CategoryName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAddBranch(string branchName, string city, string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return Json(new { success = false, message = "Şube adı boş olamaz." });

                if (string.IsNullOrWhiteSpace(city))
                    return Json(new { success = false, message = "Şehir boş olamaz." });

                var branch = new Branch
                {
                    BranchName = branchName.Trim(),
                    City = city.Trim(),
                    Address = address?.Trim() ?? ""
                };

                await _branchService.TInsertAsync(branch);

                return Json(new
                {
                    success = true,
                    id = branch.BranchId,
                    name = $"{branch.BranchName} - {branch.City}"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}