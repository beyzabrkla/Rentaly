using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DTOLayer.RentalDTOs;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public RentalController(IRentalService rentalService, ICarService carService, IBranchService branchService, IMapper mapper)
        {
            _rentalService = rentalService;
            _carService = carService;
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var car = await _carService.GetCarByIdWithDetailsAsync(id);
            if (car == null) return NotFound();

            var model = new RentalIndexViewModel
            {
                CreateRentalDto = new CreateRentalDTO
                {
                    CarId = id,
                    DailyPrice = car.DailyPrice,
                    DepositAmount = car.DepositAmount, 
                    PickupDate = DateTime.Today.AddDays(1),
                    ReturnDate = DateTime.Today.AddDays(2),
                    PickupBranchId = car.BranchId
                },

                BrandName = car.Brand.BrandName,
                ModelName = car.CarModel.ModelName,
                CoverImageUrl = car.CoverImageUrl,
                CategoryName = car.Category.CategoryName,
                PlateNumber = car.PlateNumber,
                DailyPrice = car.DailyPrice,
                DepositAmount = car.DepositAmount, 
                CurrentBranchName = car.Branch.BranchName
            };

            ViewBag.Branches = await _branchService.TGetListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(RentalIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await GetBackingDataForViewModel(model);
                ViewBag.Branches = await _branchService.TGetListAsync();
                return View(model);
            }

            // Araç müsaitlik kontrolü
            var isAvailable = await _rentalService.TCheckCarAvailabilityAsync(
                model.CreateRentalDto.CarId,
                model.CreateRentalDto.PickupDate.Value,
                model.CreateRentalDto.ReturnDate.Value);

            if (!isAvailable)
            {
                ModelState.AddModelError("", "Seçilen tarihlerde araç maalesef dolu.");
                await GetBackingDataForViewModel(model);
                ViewBag.Branches = await _branchService.TGetListAsync();
                return View(model);
            }

            var rental = _mapper.Map<Rental>(model.CreateRentalDto);

            // (GÜN * KİRA) + DEPOZİTO = TOTAL ÖDEME
            var timeSpan = model.CreateRentalDto.ReturnDate.Value - model.CreateRentalDto.PickupDate.Value;
            int totalDays = (int)Math.Ceiling(timeSpan.TotalDays);
            if (totalDays <= 0) totalDays = 1;

            decimal rentalFee = totalDays * model.CreateRentalDto.DailyPrice;

            rental.TotalPrice = rentalFee + model.CreateRentalDto.DepositAmount;

            rental.Status = "Beklemede";
            rental.IsApproved = false;
            rental.CreatedDate = DateTime.Now;

            await _rentalService.TInsertAsync(rental);

            return RedirectToAction("Confirmation", new { rentalId = rental.RentalId });
        }

        [HttpGet]
        public IActionResult Confirmation(int rentalId)
        {
            ViewBag.RentalId = rentalId;
            return View();
        }

        private async Task GetBackingDataForViewModel(RentalIndexViewModel model) // Model doğrulama başarısız olduğunda, araç bilgilerini tekrar yüklemek için yardımcı metod
        {
            var car = await _carService.GetCarByIdWithDetailsAsync(model.CreateRentalDto.CarId);
            if (car != null)
            {
                model.BrandName = car.Brand.BrandName;
                model.ModelName = car.CarModel.ModelName;
                model.CoverImageUrl = car.CoverImageUrl;
                model.CategoryName = car.Category.CategoryName;
                model.PlateNumber = car.PlateNumber;
                model.DailyPrice = car.DailyPrice;
                model.DepositAmount = car.DepositAmount;
                model.CurrentBranchName = car.Branch.BranchName;
            }
        }
    }
}