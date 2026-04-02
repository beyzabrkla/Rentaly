using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules.BrandValidator;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.BranchDTOs;
using Rentaly.DTOLayer.BrandDTOs;
using Rentaly.DTOLayer.CarDTOs;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
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
        public async Task<IActionResult> BrandList(string search, int page = 1)
        {
            try
            {
                var allBrands = await _brandService.TGetListAsync();
                var allCars = await _carService.TGetListAsync();

                var filteredBrands = allBrands.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    filteredBrands = filteredBrands.Where(x => x.BrandName.ToLower().Contains(search));
                }

                ViewBag.TotalCount = allBrands.Count;

                int pageSize = 8;
                var brandList = filteredBrands.ToList();
                int totalItems = brandList.Count;

                var pagedData = brandList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                ViewBag.CarCounts = allBrands.ToDictionary(
                     b => b.BrandId,
                     b => allCars.Count(c => c.BrandId == b.BrandId)
                 );

                return View(pagedData);
            }
            catch (Exception)
            {
                TempData["Error"] = "Markalar yüklenirken bir hata oluştu.";
                ViewBag.CarCounts = new Dictionary<int, int>();
                return View(new List<Brand>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> BrandDetail(int id)
        {
            try
            {
                var brand = await _brandService.TGetByIdAsync(id);
                if (brand == null) return NotFound("Marka bulunamadı.");

                var allCars = await _carService.TGetListAsync();
                var brandCars = allCars.Where(c => c.BrandId == id).ToList();

                var carDtoList = _mapper.Map<List<ResultCarDTO>>(brandCars);

                ViewBag.Branc = brand;
                ViewBag.TotalCars = brandCars.Count;
                ViewBag.AvailableCount = brandCars.Count(c => c.IsAvailable);
                ViewBag.RentedCount = brandCars.Count(c => !c.IsAvailable);

                return View(carDtoList);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Şube detayı yüklenirken bir hata oluştu.";
                return RedirectToAction("BranchList");
            }
        }


        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(CreateBrandDTO createBrandDTO)
        {
            var validator = new CreateBrandValidator();
            var result = validator.Validate(createBrandDTO);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(createBrandDTO);
            }

            try
            {
                var brand = _mapper.Map<Brand>(createBrandDTO);

                await _brandService.TInsertAsync(brand);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Marka başarıyla eklendi.";
                return RedirectToAction("BrandList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veritabanına kaydedilirken hata: " + ex.Message;
                return View(createBrandDTO);

            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBranch(int id)
        {
            var brand = await _brandService.TGetByIdAsync(id);
            if (brand == null) return NotFound();

            var dto = _mapper.Map<UpdateBrandDTO>(brand);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrand(UpdateBrandDTO updateBrandDTO)
        {
            var validator = new UpdateBrandValidator();
            var result = validator.Validate(updateBrandDTO);


            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(updateBrandDTO);
            }

            try
            {
                var brand = _mapper.Map<Brand>(updateBrandDTO);

                await _brandService.TUpdateAsync(brand);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Marka başarıyla güncellendi.";
                return RedirectToAction("BrandList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return View(updateBrandDTO);
            }
        }

    }
}
