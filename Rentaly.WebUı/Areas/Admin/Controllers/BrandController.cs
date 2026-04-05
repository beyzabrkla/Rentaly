using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules.BrandValidator;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.BrandDTOs;
using Rentaly.DTOLayer.CarDTOs;
using Rentaly.EntityLayer.Entities;

[Area("Admin")]
public class BrandController : Controller
{
    private readonly IBrandService _brandService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICarService _carService;
    private readonly IMapper _mapper;

    public BrandController(IBrandService brandService, IUnitOfWork unitOfWork, ICarService carService, IMapper mapper)
    {
        _brandService = brandService;
        _unitOfWork = unitOfWork;
        _carService = carService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> BrandList(string search, string status, int page = 1)
    {
        var allBrands = await _brandService.TGetListAsync();
        var allCars = await _carService.TGetListAsync();

        var filteredBrands = allBrands.AsQueryable();

        // Arama Filtresi
        if (!string.IsNullOrEmpty(search))
            filteredBrands = filteredBrands.Where(x => x.BrandName.ToLower().Contains(search.ToLower()));

        // Durum Filtresi
        if (!string.IsNullOrEmpty(status))
            filteredBrands = filteredBrands.Where(x => x.Status == (status == "active"));

        int pageSize = 8;
        var brandList = filteredBrands.ToList();
        var pagedData = brandList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // Pagination ve İstatistik Bilgileri
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(brandList.Count / (double)pageSize);

        // Her markanın kaç aracı olduğunu sözlüğe aktar
        ViewBag.CarCounts = allBrands.ToDictionary(
            b => b.BrandId,
            b => allCars.Count(c => c.BrandId == b.BrandId)
        );

        return View(_mapper.Map<List<ResultBrandDTO>>(pagedData));
    }

    [HttpGet]
    public async Task<IActionResult> BrandDetail(int id)
    {
        var brand = await _brandService.TGetByIdAsync(id);
        if (brand == null) return NotFound();

        var allCars = await _carService.TGetListAsync();
        var brandCars = allCars.Where(x => x.BrandId == id).ToList();

        ViewBag.BrandName = brand.BrandName;
        ViewBag.TotalCars = brandCars.Count;
        ViewBag.AvailableCount = brandCars.Count(c => c.IsAvailable);
        ViewBag.RentedCount = brandCars.Count(c => !c.IsAvailable);

        var mappedCars = _mapper.Map<List<ResultCarDTO>>(brandCars);
        return View(mappedCars);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBrand(CreateBrandDTO createBrandDTO)
    {
        if (string.IsNullOrWhiteSpace(createBrandDTO?.BrandName))
            return Json(new
            {
                success = false,
                errors = new[] {
            new { propertyName = "BrandName", errorMessage = "Marka adı boş geçilemez" }
            }
            });

        var validator = new CreateBrandValidator();
        var result = validator.Validate(createBrandDTO);
        if (!result.IsValid)
            return Json(new
            {
                success = false,
                errors = result.Errors.Select(x => new
                {
                    propertyName = x.PropertyName,
                    errorMessage = x.ErrorMessage
                }).ToList()
            });

        try
        {
            var brand = _mapper.Map<Brand>(createBrandDTO);
            brand.Status = createBrandDTO.Status ?? true;
            await _brandService.TInsertAsync(brand);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                errors = new[] {
            new { propertyName = "BrandName", errorMessage = ex.Message.Contains("zaten") ? ex.Message : "Bir hata oluştu." }
        }
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBrand(UpdateBrandDTO updateBrandDTO)
    {
        if (string.IsNullOrWhiteSpace(updateBrandDTO?.BrandName))
            return Json(new
            {
                success = false,
                errors = new[] {
            new { propertyName = "BrandName", errorMessage = "Marka adı boş geçilemez" }
        }
            });

        var validator = new UpdateBrandValidator();
        var result = validator.Validate(updateBrandDTO);
        if (!result.IsValid)
            return Json(new
            {
                success = false,
                errors = result.Errors.Select(x => new
                {
                    propertyName = x.PropertyName,
                    errorMessage = x.ErrorMessage
                }).ToList()
            });

        try
        {
            var brand = await _brandService.TGetByIdAsync(updateBrandDTO.BrandId);
            if (brand == null)
                return Json(new
                {
                    success = false,
                    errors = new[] {
                new { propertyName = "BrandName", errorMessage = "Marka bulunamadı." }
            }
                });

            var currentStatus = brand.Status;
            _mapper.Map(updateBrandDTO, brand);
            brand.Status = currentStatus;

            if (string.IsNullOrWhiteSpace(brand.BrandName))
                return Json(new
                {
                    success = false,
                    errors = new[] {
                new { propertyName = "BrandName", errorMessage = "Marka adı boş geçilemez" }
            }
                });

            await _brandService.TUpdateAsync(brand);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                errors = new[] {
            new { propertyName = "BrandName", errorMessage = ex.Message.Contains("zaten") ? ex.Message : "Bir hata oluştu." }
        }
            });
        }
    }

    public async Task<IActionResult> ChangeStatus(int id)
    {
        var brand = await _brandService.TGetByIdAsync(id);
        if (brand != null)
        {
            brand.Status = !brand.Status;
            await _brandService.TUpdateAsync(brand);
            await _unitOfWork.SaveAsync();
            TempData["Success"] = "Marka durumu güncellendi.";
        }
        return RedirectToAction("BrandList");
    }
}