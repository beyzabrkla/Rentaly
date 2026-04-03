using AutoMapper;
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
        var validator = new CreateBrandValidator();
        var result = validator.Validate(createBrandDTO);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors) { ModelState.AddModelError(error.PropertyName, error.ErrorMessage); }
            return View(createBrandDTO);
        }

        var brand = _mapper.Map<Brand>(createBrandDTO);
        await _brandService.TInsertAsync(brand);
        await _unitOfWork.SaveAsync();
        TempData["Success"] = "Marka başarıyla eklendi.";
        return RedirectToAction("BrandList");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBrand(UpdateBrandDTO updateBrandDTO)
    {
        var validator = new UpdateBrandValidator();
        var result = validator.Validate(updateBrandDTO);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors) { ModelState.AddModelError(error.PropertyName, error.ErrorMessage); }
            return View(updateBrandDTO);
        }

        var brand = await _brandService.TGetByIdAsync(updateBrandDTO.BrandId);
        if (brand == null) return NotFound();

        _mapper.Map(updateBrandDTO, brand);
        await _brandService.TUpdateAsync(brand);
        await _unitOfWork.SaveAsync();

        TempData["Success"] = "Marka başarıyla güncellendi.";
        return RedirectToAction("BrandList");
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