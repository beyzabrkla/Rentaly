using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules.CarValidator;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.CarDTOs;
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
        private readonly IMapper _mapper;

        public CarController(
            ICarService carService,
            IBrandService brandService,
            ICarModelService carModelService,
            ICategoryService categoryService,
            IBranchService branchService,
            IUnitOfWork unitOfWork,
            ICarImageService carImageService,
            IMapper mapper)
        {
            _carService = carService;
            _brandService = brandService;
            _carModelService = carModelService;
            _categoryService = categoryService;
            _branchService = branchService;
            _unitOfWork = unitOfWork;
            _carImageService = carImageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CarList(int page = 1, string? search = null, int? brand = null, string? fuel = null, string? status = null, string? active = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            try
            {
                var allCars = await _carService.GetAllWithDetailsAsync();
                var query = allCars.AsQueryable();

                // Özet İstatistikler (Filtrelemeden bağımsız)
                ViewBag.TotalCars = allCars.Count;
                ViewBag.AvailableCount = allCars.Count(c => c.IsAvailable && c.IsActive);
                ViewBag.RentedCount = allCars.Count(c => !c.IsAvailable);
                ViewBag.PassiveCount = allCars.Count(c => !c.IsActive);

                // Arama Filtresi
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(c =>
                        (c.Brand != null && c.Brand.BrandName.ToLower().Contains(search)) ||
                        (c.PlateNumber != null && c.PlateNumber.ToLower().Contains(search)));
                }

                // --- DÜZELTİLEN KISIM: Kiralama Durumu Filtresi ---
                // JS'den "true" (Müsait) veya "false" (Kirada) geliyor
                if (!string.IsNullOrEmpty(status))
                {
                    if (status.ToLower() == "true")
                        query = query.Where(c => c.IsAvailable);
                    else if (status.ToLower() == "false")
                        query = query.Where(c => !c.IsAvailable);
                }

                // Yayın Durumu Filtresi (IsActive)
                if (!string.IsNullOrEmpty(active))
                {
                    query = query.Where(c => active.ToLower() == "true" ? c.IsActive : !c.IsActive);
                }

                // Marka Filtresi (Eksikti, eklendi)
                if (brand.HasValue)
                {
                    query = query.Where(c => c.BrandId == brand.Value);
                }

                // Yakıt Filtresi (Eksikti, eklendi)
                if (!string.IsNullOrEmpty(fuel))
                {
                    query = query.Where(c => c.FuelType == fuel);
                }

                // Fiyat Filtreleri
                if (minPrice.HasValue) query = query.Where(c => c.DailyPrice >= minPrice.Value);
                if (maxPrice.HasValue) query = query.Where(c => c.DailyPrice <= maxPrice.Value);

                // Sayfalama işlemleri
                int totalFiltered = query.Count();
                int totalPages = (int)Math.Ceiling(totalFiltered / (double)PAGE_SIZE);
                page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

                var pagedCars = query.OrderByDescending(x => x.CreatedDate)
                                     .Skip((page - 1) * PAGE_SIZE)
                                     .Take(PAGE_SIZE)
                                     .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.Brands = await _brandService.TGetListAsync();

                // Mevcut filtreleri View'da korumak için (Opsiyonel ama önerilir)
                ViewBag.CurrentStatus = status;
                ViewBag.CurrentBrand = brand;

                return View(_mapper.Map<List<ResultCarDTO>>(pagedCars));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç listesi yüklenirken teknik bir hata oluştu.";
                return View(new List<ResultCarDTO>());
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
        public async Task<IActionResult> CreateCar(CreateCarDTO createCarDTO, IFormFile? imageFile, List<IFormFile>? carImageFiles, string? imageSourceType)
        {
            try
            {
                var validator = new CreateCarValidator();
                var result = await validator.ValidateAsync(createCarDTO);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    ViewBag.Brands = await _brandService.TGetListAsync();
                    ViewBag.Categories = await _categoryService.TGetListAsync();
                    ViewBag.Branches = await _branchService.TGetListAsync();
                    return View(createCarDTO);
                }

                // GÖRSEL KONTROLÜ
                bool hasFile = imageFile != null && imageFile.Length > 0;
                bool hasUrl = imageSourceType == "url" && !string.IsNullOrEmpty(createCarDTO.CoverImageUrl);

                var car = _mapper.Map<Car>(createCarDTO);
                car.CreatedDate = DateTime.Now;
                await _carService.TInsertAsync(car);
                await _unitOfWork.SaveAsync();

                // KAPAK GÖRSELİ
                if (hasFile)
                {
                    if (imageFile!.Length <= 5 * 1024 * 1024)
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var fullPath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                            await imageFile.CopyToAsync(stream);

                        car.CoverImageUrl = $"/uploads/cars/{fileName}";
                        await _carService.TUpdateAsync(car);
                        await _unitOfWork.SaveAsync();
                    }
                }
                else if (hasUrl)
                {
                    car.CoverImageUrl = createCarDTO.CoverImageUrl;
                    await _carService.TUpdateAsync(car);
                    await _unitOfWork.SaveAsync();
                }

                // DİĞER GÖRSELLER
                if (carImageFiles != null && carImageFiles.Count > 0)
                {
                    foreach (var file in carImageFiles)
                    {
                        if (file.Length <= 0 || file.Length > 5 * 1024 * 1024)
                            continue;

                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cars");

                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        var fullPath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        await _carImageService.TInsertAsync(new CarImage
                        {
                            CoverImageUrl = $"/uploads/cars/{fileName}",
                            CarId = car.CarId
                        });
                    }

                    await _unitOfWork.SaveAsync();
                }

                TempData["Success"] = "Araç başarıyla kaydedildi.";
                return RedirectToAction("CarList", "Car", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();

                // Hatanın tamamını görmek için:
                TempData["Error"] = $"HATA: {ex.ToString()}";
                return View(createCarDTO);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditCar(int id)
        {
            try
            {
                var car = await _carService.GetCarByIdWithDetailsAsync(id);
                if (car == null) return NotFound("Araç bulunamadı.");

                var dto = _mapper.Map<UpdateCarDTO>(car);

                await LoadEditViewBags(car.BrandId);

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Araç yüklenirken hata oluştu.";
                return RedirectToAction("CarList", "Car", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(
            int id,
            UpdateCarDTO updateCarDTO,
            IFormFile? imageFile,
            List<IFormFile>? carImageFiles,
            string? imageSourceType,
            List<string>? deletedImageIds)
        {
            try
            {
                var car = await _carService.TGetByIdAsync(id);
                if (car == null) return NotFound("Güncellenecek araç bulunamadı.");

                string oldCoverPath = car.CoverImageUrl;
                DateTime originalCreatedDate = car.CreatedDate; //mevcut tarihi yedekle

                var validator = new UpdateCarValidator();
                var validationResult = await validator.ValidateAsync(updateCarDTO);
                
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    await LoadEditViewBags(updateCarDTO.BrandId);
                    return View(updateCarDTO);
                }

                //mapping
                _mapper.Map(updateCarDTO, car);

                car.CreatedDate = originalCreatedDate; //yedeklenen tarihi geri yükle

                // Kapak Görseli
                if (imageSourceType == "remove")
                {
                    DeleteLocalFile(oldCoverPath);
                    car.CoverImageUrl = null;
                }
                else if (imageSourceType == "file" && imageFile != null && imageFile.Length > 0) // Yeni dosya gelmişse veya var olan dosya değiştirilmek isteniyorsa
                {
                    // Yeni dosya yükleniyor, eskisini temizle
                    DeleteLocalFile(oldCoverPath);

                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/cars");

                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                    var fullPath = Path.Combine(directoryPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    car.CoverImageUrl = $"/uploads/cars/{fileName}";
                }
                else if (imageSourceType == "url" && !string.IsNullOrEmpty(updateCarDTO.CoverImageUrl))
                {
                    // URL girildiyse Mapper zaten car.CoverImageUrl'i set etti, dokunmuyoruz.
                }
                else
                {
                    // Hiçbir değişiklik yoksa yedeklediğimiz eski yolu geri veriyoruz
                    car.CoverImageUrl = oldCoverPath;
                }

                //Silinecek Galeri Görselleri
                if (deletedImageIds != null && deletedImageIds.Any())
                {
                    foreach (var imgId in deletedImageIds)
                    {
                        if (int.TryParse(imgId, out int parsedId))
                        {
                            var img = await _carImageService.TGetByIdAsync(parsedId);
                            if (img != null)
                            {
                                DeleteLocalFile(img.CoverImageUrl);
                                await _carImageService.TDeleteAsync(parsedId);
                            }
                        }
                    }
                }

                if (carImageFiles != null && carImageFiles.Any())
                {
                    foreach (var file in carImageFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/cars", fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                                await file.CopyToAsync(stream);

                            await _carImageService.TInsertAsync(new CarImage
                            {
                                CoverImageUrl = $"/uploads/cars/{fileName}",
                                CarId = car.CarId
                            });
                        }
                    }
                }

                await _carService.TUpdateAsync(car);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Araç bilgileri başarıyla güncellendi.";
                return RedirectToAction("CarList", "Car", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                await LoadEditViewBags(updateCarDTO.BrandId);
                TempData["Error"] = $"Güncelleme sırasında hata oluştu: {ex.Message} | Inner: {ex.InnerException?.Message}";
                ModelState.AddModelError("", ex.Message);
                return View(updateCarDTO);
            }
        }

        private async Task LoadEditViewBags(int brandId)
        {
            ViewBag.Brands = await _brandService.TGetListAsync();
            ViewBag.Categories = await _categoryService.TGetListAsync();
            ViewBag.Branches = await _branchService.TGetListAsync();
            if (brandId > 0)
                ViewBag.Models = await _carModelService.GetModelsByBrandAsync(brandId);
        }

        private void DeleteLocalFile(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("/"))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                //Aracı, ilişkili resim listesiyle çekiyoruz 
                var car = await _carService.GetCarByIdWithDetailsAsync(id);
                if (car == null)
                    return Json(new { success = false, message = "Araç bulunamadı." });

                //Önce galeri Resimleri temizleniyor
                if (car.CarImages != null && car.CarImages.Any())
                {
                    foreach (var img in car.CarImages.ToList())
                    {
                        // Fiziksel dosyayı wwwroot'tan sil
                        DeleteLocalFile(img.CoverImageUrl);
                        await _carImageService.TDeleteAsync(img.CarImageId);
                    }
                }

                //Aracın ana kapak fotoğrafını sil
                DeleteLocalFile(car.CoverImageUrl);
                await _carService.TDeleteAsync(id);

                await _unitOfWork.SaveAsync();

                return Json(new { success = true, message = "Araç ve tüm görselleri başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Silme işlemi başarısız: {ex.Message}",
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetModelsByBrand(int brandId)
        {
            try
            {
                var models = await _carModelService.GetModelsByBrandAsync(brandId);
                return Json(new
                {
                    success = true,
                    data = models.Select(m => new { id = m.CarModelId, name = m.ModelName })
                });
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
                await _unitOfWork.SaveAsync();
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
                await _unitOfWork.SaveAsync();
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
                await _unitOfWork.SaveAsync();
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
                await _unitOfWork.SaveAsync();
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