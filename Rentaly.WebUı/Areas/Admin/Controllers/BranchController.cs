using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly ICarService _carService;
        private readonly IUnitOfWork _unitOfWork;

        public BranchController(
            IBranchService branchService,
            ICarService carService,
            IUnitOfWork unitOfWork)
        {
            _branchService = branchService;
            _carService = carService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> BranchList(string search, bool? status, int page = 1)
        {
            try
            {
                var allBranches = await _branchService.TGetListAsync();
                var allCars = await _carService.TGetListAsync();

                // Filtreleme Mantığı
                var filteredBranches = allBranches.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    filteredBranches = filteredBranches.Where(x =>
                        x.BranchName.ToLower().Contains(search) ||
                        x.City.ToLower().Contains(search)
                    ).AsQueryable();
                }

                if (status.HasValue)
                {
                    filteredBranches = filteredBranches.Where(x => x.IsActive == status.Value).AsQueryable();
                }

                // İstatistikler (Filtrelemeden bağımsız genel sayılar)
                ViewBag.TotalCount = allBranches.Count;
                ViewBag.ActiveCount = allBranches.Count(x => x.IsActive);
                ViewBag.PassiveCount = allBranches.Count(x => !x.IsActive);

                // Sayfalama Mantığı (6 tane)
                int pageSize = 6;
                var branchList = filteredBranches.ToList();
                int totalItems = branchList.Count;

                var pagedData = branchList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // View'a Gidecek Diğer Veriler
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                ViewBag.CarCounts = allBranches.ToDictionary(
                    b => b.BranchId,
                    b => allCars.Count(c => c.BranchId == b.BranchId)
                );

                return View(pagedData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Şubeler yüklenirken bir hata oluştu.";
                ViewBag.CarCounts = new Dictionary<int, int>();
                return View(new List<Branch>());
            }
        }
    
        [HttpGet]
        public async Task<IActionResult> BranchDetail(int id)
        {
            try
            {
                var branch = await _branchService.TGetByIdAsync(id);
                if (branch == null)
                    return NotFound("Şube bulunamadı.");

                var allCars = await _carService.TGetListAsync();
                var branchCars = allCars.Where(c => c.BranchId == id).ToList();

                ViewBag.Branch = branch;
                ViewBag.TotalCars = branchCars.Count;
                ViewBag.AvailableCount = branchCars.Count(c => c.IsAvailable);
                ViewBag.RentedCount = branchCars.Count(c => !c.IsAvailable);

                return View(branchCars);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Şube detayı yüklenirken bir hata oluştu.";
                return RedirectToAction("BranchList");
            }
        }

        [HttpGet]
        public IActionResult CreateBranch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBranch([FromBody]Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branch);

                await _branchService.TInsertAsync(branch);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Şube başarıyla eklendi.";
                return RedirectToAction("BranchList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata oluştu: {ex.Message}";
                return View(branch);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBranch(int id)
        {
            try
            {
                var branch = await _branchService.TGetByIdAsync(id);
                if (branch == null)
                    return NotFound("Şube bulunamadı.");

                return View(branch);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Şube yüklenirken hata oluştu.";
                return RedirectToAction("BranchList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBranch(Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branch);

                await _branchService.TUpdateAsync(branch);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Şube başarıyla güncellendi.";
                return RedirectToAction("BranchList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return View(branch);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBranch(int id)
        {
            try
            {
                var branch = await _branchService.TGetByIdAsync(id);
                if (branch == null)
                    return Json(new { success = false, message = "Şube bulunamadı." });

                branch.IsActive = !branch.IsActive;
                await _branchService.TUpdateAsync(branch);
                await _unitOfWork.SaveAsync();

                var status = branch.IsActive ? "aktif" : "pasif";
                return Json(new { success = true, message = $"Şube {status} edildi.", isActive = branch.IsActive });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata: {ex.Message}" });
            }
        }
    }
}