using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.ValidationRules.BranchValidator;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.BranchDTOs;
using Rentaly.DTOLayer.CarDTOs;
using Rentaly.EntityLayer.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly ICarService _carService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchController(
            IBranchService branchService,
            ICarService carService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _branchService = branchService;
            _carService = carService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> BranchList(string search, bool? status, int page = 1)
        {
            try
            {
                var allBranches = await _branchService.TGetListAsync();
                var allCars = await _carService.TGetListAsync();

                var filteredBranches = allBranches.AsQueryable();

                // Filtreleme
                if (!string.IsNullOrEmpty(search))
                {
                    var lowerSearch = search.ToLower();
                    filteredBranches = filteredBranches.Where(x =>
                        x.BranchName.ToLower().Contains(lowerSearch) ||
                        x.City.ToLower().Contains(lowerSearch));
                }

                if (status.HasValue)
                {
                    filteredBranches = filteredBranches.Where(x => x.IsActive == status.Value);
                }

                // Sayfalama
                int pageSize = 6;
                var branchList = filteredBranches.ToList();
                int totalItems = branchList.Count;

                var pagedBranches = branchList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagedDataDto = _mapper.Map<List<ResultBranchDTO>>(pagedBranches);

                // ViewBags
                ViewBag.TotalCount = allBranches.Count;
                ViewBag.ActiveCount = allBranches.Count(x => x.IsActive);
                ViewBag.PassiveCount = allBranches.Count(x => !x.IsActive);
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                ViewBag.Search = search;
                ViewBag.Status = status;

                ViewBag.CarCounts = allBranches.ToDictionary(
                    b => b.BranchId,
                    b => allCars.Count(c => c.BranchId == b.BranchId)
                );

                return View(pagedDataDto);
            }
            catch (Exception)
            {
                TempData["Error"] = "Şubeler yüklenirken bir hata oluştu.";
                return View(new List<ResultBranchDTO>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBranch(CreateBranchDTO createBranchDTO)
        {
            // Validasyon kontrolü
            var validator = new CreateBranchValidator();
            var result = await validator.ValidateAsync(createBranchDTO);

            if (!result.IsValid)
            {
                TempData["Error"] = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                return RedirectToAction("BranchList");
            }

            try
            {
                var branch = _mapper.Map<Branch>(createBranchDTO);
                await _branchService.TInsertAsync(branch);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Yeni şube başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata: " + ex.Message;
            }

            return RedirectToAction("BranchList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBranch(UpdateBranchDTO updateBranchDTO)
        {
            var validator = new UpdateBranchValidator();
            var result = await validator.ValidateAsync(updateBranchDTO);

            if (!result.IsValid)
            {
                TempData["Error"] = string.Join("<br>", result.Errors.Select(x => x.ErrorMessage));
                return RedirectToAction("BranchList");
            }

            try
            {
                var branch = await _branchService.TGetByIdAsync(updateBranchDTO.BranchId);
                if (branch == null) return NotFound();

                _mapper.Map(updateBranchDTO, branch);
                await _branchService.TUpdateAsync(branch);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Şube bilgileri güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme sırasında hata: " + ex.Message;
            }

            return RedirectToAction("BranchList");
        }

        [HttpGet]
        public async Task<IActionResult> BranchDetail(int id, string search, string fuel, string transmission, int page = 1)
        {
            //Şube Bilgisini Getir
            var branch = await _branchService.TGetByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            ViewBag.Branch = branch;

            //Şubeye Ait Araçları Getir (İlişkili verilerle birlikte)
            var cars = await _carService.GetCarsByBranchWithDetailsAsync(id);

            //İstatistikleri (Filtresiz ham veri üzerinden) hesapla
            ViewBag.TotalCars = cars.Count;
            ViewBag.AvailableCount = cars.Count(x => x.IsAvailable);
            ViewBag.RentedCount = cars.Count(x => !x.IsAvailable);

            //Filtreleme İşlemleri
            var filteredCars = cars.AsEnumerable();

            if (!string.IsNullOrEmpty(search))
            {
                filteredCars = filteredCars.Where(x =>
                    (x.Brand != null && x.Brand.BrandName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (x.CarModel != null && x.CarModel.ModelName.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(fuel))
            {
                filteredCars = filteredCars.Where(x => x.FuelType == fuel);
            }

            if (!string.IsNullOrEmpty(transmission))
            {
                filteredCars = filteredCars.Where(x => x.Transmission == transmission);
            }

            int pageSize = 6;
            var totalFilteredCount = filteredCars.Count();

            var pagedCars = filteredCars
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedCars = _mapper.Map<List<ResultCarDTO>>(pagedCars);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalFilteredCount / pageSize);
            ViewBag.Search = search;
            ViewBag.Fuel = fuel;
            ViewBag.Transmission = transmission;

            return View(mappedCars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBranch(int id)
        {
            try
            {
                var branch = await _branchService.TGetByIdAsync(id);
                if (branch == null) return Json(new { success = false });

                branch.IsActive = !branch.IsActive;
                await _branchService.TUpdateAsync(branch);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, isActive = branch.IsActive });
            }
            catch { return Json(new { success = false }); }
        }
    }
}