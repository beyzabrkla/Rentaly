using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.CarModelDTOs;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;
        private readonly IBrandService _brandService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarModelController(ICarModelService carModelService, IBrandService brandService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _carModelService = carModelService;
            _brandService = brandService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> CarModelList(int page = 1, string? search = null, int? brand = null)
        {
            const int PAGE_SIZE = 8;
            var allModels = await _carModelService.GetCarModelsWithBrandsAsync();
            var query = allModels.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.ModelName.Contains(search, StringComparison.OrdinalIgnoreCase));

            if (brand.HasValue && brand.Value > 0)
                query = query.Where(x => x.BrandId == brand.Value);

            int totalFiltered = query.Count();
            int totalPages = (int)Math.Ceiling(totalFiltered / (double)PAGE_SIZE);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

            var pagedModels = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();
            var modelDtoList = _mapper.Map<List<ResultCarModelDTO>>(pagedModels);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Brands = await _brandService.TGetListAsync();
            ViewBag.CurrentBrand = brand;

            return View(modelDtoList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel(CreateCarModelDTO createCarModelDTO)
        {
            var model = _mapper.Map<CarModel>(createCarModelDTO);
            await _carModelService.TInsertAsync(model);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("CarModelList");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(UpdateCarModelDTO updateCarModelDTO)
        {
            ModelState.Remove("BrandName");

            if (ModelState.IsValid)
            {
                var model = _mapper.Map<CarModel>(updateCarModelDTO);
                await _carModelService.TUpdateAsync(model);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("CarModelList");
            }

            return RedirectToAction("CarModelList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var value = await _carModelService.TGetByIdAsync(id);
            if (value != null)
            {
                await _carModelService.TDeleteAsync(id);
                await _unitOfWork.SaveAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}