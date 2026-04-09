using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.DTOLayer.RentalDTOs;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public RentalController(IRentalService rentalService, IMapper mapper, IUnitOfWork unitOfWork, IMailService mailService, ICarService carService)
        {
            _rentalService = rentalService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> RentalList(string? status = null, string? search = null, int page = 1)
        {
            const int pageSize = 4;

            try
            {
                //Veriyi tüm detaylarıyla (Car, Brand, Branch vb.) çekiyoruz
                var allRentals = await _rentalService.TGetListWithDetailsAsync();
                var query = allRentals.AsQueryable();

                //İstatistikler (Filtreleme öncesi)
                ViewBag.TotalCount = allRentals.Count;
                ViewBag.PendingCount = allRentals.Count(x => x.Status == "Beklemede");
                ViewBag.ApprovedCount = allRentals.Count(x => x.Status == "Onaylandı");
                ViewBag.CancelledCount = allRentals.Count(x => x.Status == "İptal Edildi");

                //Filtreleme Mantığı
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(x => x.Status == status);

                if (!string.IsNullOrEmpty(search))
                {
                    var s = search.ToLower();
                    query = query.Where(x =>
                        (x.Name + " " + x.Surname).ToLower().Contains(s) ||
                        (x.IdentityNumber != null && x.IdentityNumber.Contains(s)) ||
                        (x.Car != null && x.Car.PlateNumber != null && x.Car.PlateNumber.ToLower().Contains(s)));
                }

                //Sayfalama Hesaplamaları
                int totalRecords = query.Count();
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                if (page < 1) page = 1;
                if (totalPages > 0 && page > totalPages) page = totalPages;

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalRecords = totalRecords;

                //Veriyi Sayfalayıp DTO'ya Map'liyoruz
                var paged = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var mappedList = _mapper.Map<List<ResultRentalDTO>>(paged);

                // PropertyNamingPolicy = null sayesinde "PickupBranchName" ismi JSON'da da aynen korunur.
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = null
                };
                ViewBag.RentalJson = System.Text.Json.JsonSerializer.Serialize(mappedList, jsonOptions);

                return View(mappedList);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veriler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<ResultRentalDTO>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRentalStatus(ChangeRentalStatusDTO updateDto)
        {
            try
            {
                // Tüm listeyi çekmek yerine sadece ilgili Rental kaydını detaylarıyla getiriyoruz
                var currentRental = await _rentalService.TGetByIdAsync(updateDto.RentalId);

                if (currentRental == null)
                    return Json(new { success = false, message = "Kiralama kaydı bulunamadı!" });

                // İlgili aracı getir
                var car = await _carService.TGetByIdAsync(currentRental.CarId);
                if (car == null)
                    return Json(new { success = false, message = "Bu kiralamaya ait araç sistemde bulunamadı!" });

                // --- DURUM YÖNETİMİ ---

                if (updateDto.Status == "Onaylandı")
                {
                    // Eğer araç zaten başka bir rezervasyonla kiralandıysa (IsAvailable == false) engelle
                    if (!car.IsAvailable)
                    {
                        return Json(new { success = false, message = "Bu araç şu an başka bir müşteride veya kirada görünüyor!" });
                    }

                    car.IsAvailable = false; // Artık müsait değil
                    await _carService.TUpdateAsync(car);

                    // Müşteri Kayıt Kontrolü (Aynı identityNumber ile daha önce gelmiş mi?)
                    var existingCustomer = await _unitOfWork.Context.Customers
                        .FirstOrDefaultAsync(x => x.IdentityNumber == currentRental.IdentityNumber);

                    if (existingCustomer == null)
                    {
                        var newCustomer = new Rentaly.EntityLayer.Entities.Customer
                        {
                            Name = currentRental.Name,
                            Surname = currentRental.Surname,
                            IdentityNumber = currentRental.IdentityNumber,
                            Email = currentRental.Email,
                            Phone = currentRental.Phone,
                            DrivingLicenseNumber = currentRental.DrivingLicenseNumber,
                            DrivingLicenseDate = DateTime.Now, // Gerekirse DTO'dan tarih alınabilir
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        await _unitOfWork.Context.Customers.AddAsync(newCustomer);
                    }
                }
                else if (updateDto.Status == "İptal Edildi" || updateDto.Status == "Tamamlandı")
                {
                    // Rezervasyon iptal olursa veya araç geri teslim edilirse (Tamamlandı) müsait yap
                    car.IsAvailable = true;
                    await _carService.TUpdateAsync(car);
                }

                // --- REZERVASYON GÜNCELLEME ---
                currentRental.Status = updateDto.Status;
                currentRental.AdminNote = updateDto.AdminNote;
                currentRental.IsApproved = (updateDto.Status == "Onaylandı");

                await _rentalService.TUpdateAsync(currentRental);

                // UnitOfWork sayesinde tüm işlemler (Müşteri ekleme, Araç durum, Rental durum) tek transaction'da biter.
                await _unitOfWork.SaveAsync();

                // --- MAIL GÖNDERİMİ ---
                if (updateDto.Status == "Onaylandı")
                {
                    // Mail hatası tüm işlemi patlatmasın diye try-catch içinde bırakıyoruz
                    _ = Task.Run(async () => {
                        try { await _mailService.SendReservationApprovalMailAsync(currentRental); }
                        catch (Exception mailEx) { /* Loglama yapılabilir */ }
                    });
                }

                return Json(new
                {
                    success = true,
                    message = $"İşlem başarılı. Araç durumu: {(car.IsAvailable ? "Müsait" : "Kiralandı")} olarak güncellendi."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Teknik bir hata oluştu: " + ex.Message });
            }
        }
    }
}