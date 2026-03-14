using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.DataAccessLayer.UnitOfWork;

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
        public async Task<IActionResult> CarList()
        {
            try
            {
                // Tüm araçları getir (silinen hariç)
                var cars = await _carService.TGetListAsync();
                return View(cars);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Araçlar yüklenirken hata oluştu: {ex.Message}";
                return View(new List<Car>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            try
            {
                // ViewBag ile dropdown verileri gönder
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
        public async Task<IActionResult> CreateCar(Car car, List<IFormFile> imageFiles)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Brands = await _brandService.TGetListAsync();
                    ViewBag.Categories = await _categoryService.TGetListAsync();
                    ViewBag.Branches = await _branchService.TGetListAsync();
                    return View(car);
                }

                //Önce arabayı kaydet ki CarId oluşsun
                await _carService.TInsertAsync(car);
                await _unitOfWork.SaveAsync();

                // Fotoğrafları döngüye al ve kaydet
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    foreach (var file in imageFiles)
                    {
                        if (file.Length > 0 && file.Length <= 5 * 1024 * 1024)
                        {
                            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);

                            var directory = Path.GetDirectoryName(uploadPath);
                            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                            using (var stream = new FileStream(uploadPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // CarImage nesnesini oluştur ve veritabanına ekle
                            var carImage = new CarImage
                            {
                                ImageUrl = $"/uploads/cars/{fileName}",
                                CarId = car.CarId
                            };

                            await _carImageService.TInsertAsync(carImage);
                        }
                    }
                    await _unitOfWork.SaveAsync(); // Tüm resim kayıtlarını tek seferde işle
                }

                TempData["Success"] = "Araç ve fotoğrafları başarıyla kaydedildi.";
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
                var car = await _carService.TGetByIdAsync(id);
                if (car == null)
                {
                    return NotFound("Araç bulunamadı.");
                }

                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();

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
        public async Task<IActionResult> EditCar(int id, Car car, IFormFile imageFile)
        {
            try
            {
                if (id != car.CarId)
                {
                    return BadRequest("Araç ID eşleşmiyor.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Brands = await _brandService.TGetListAsync();
                    ViewBag.Categories = await _categoryService.TGetListAsync();
                    ViewBag.Branches = await _branchService.TGetListAsync();
                    return View(car);
                }

                // Eğer yeni görsel seçildiyse, eskisini sil ve yenisini kaydet
                if (imageFile != null && imageFile.Length > 0)
                {
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["Error"] = "Görsel dosyası 5MB'dan büyük olamaz.";
                        ViewBag.Brands = await _brandService.TGetListAsync();
                        ViewBag.Categories = await _categoryService.TGetListAsync();
                        ViewBag.Branches = await _branchService.TGetListAsync();
                        return View(car);
                    }

                    // Eski görseli sil
                    if (!string.IsNullOrEmpty(car.ImageUrl))
                    {
                        var oldImagePath = Path.Combine("wwwroot", car.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Yeni görseli kaydet
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                    var uploadPath = Path.Combine("wwwroot", "uploads", "cars", fileName);

                    var directory = Path.GetDirectoryName(uploadPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    car.ImageUrl = $"/uploads/cars/{fileName}";
                }

                await _carService.TUpdateAsync(car);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Araç başarıyla güncellendi.";
                return RedirectToAction("CarList");
            }
            catch (Exception ex)
            {
                ViewBag.Brands = await _brandService.TGetListAsync();
                ViewBag.Categories = await _categoryService.TGetListAsync();
                ViewBag.Branches = await _branchService.TGetListAsync();
                TempData["Error"] = $"Araç güncellenirken hata oluştu: {ex.Message}";
                return View(car);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _carService.TGetByIdAsync(id);
                if (car == null)
                {
                    return Json(new { success = false, message = "Araç bulunamadı." });
                }

                // Görseli sil
                if (!string.IsNullOrEmpty(car.ImageUrl))
                {
                    var imagePath = Path.Combine("wwwroot", car.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Araçı sil
                await _carService.TDeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, message = "Araç başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Araç silinirken hata oluştu: {ex.Message}" });
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
    }
}