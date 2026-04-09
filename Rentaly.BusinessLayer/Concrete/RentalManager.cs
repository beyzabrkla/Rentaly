using Microsoft.EntityFrameworkCore;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

public class RentalManager : GenericManager<Rental>, IRentalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;

    public RentalManager(IUnitOfWork unitOfWork, IMailService mailService) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public async Task<List<Rental>> TGetRentalsWithDetailsAsync()
    {
        // DbContext'e doğrudan erişim — UnitOfWork üzerinden context al
        var context = _unitOfWork.Context; // Bunu aşağıda açıklıyorum
        return await context.Rentals
            .Include(x => x.Car)
                .ThenInclude(c => c.Brand)
            .Include(x => x.Car)
                .ThenInclude(c => c.CarModel)
            .Include(x => x.PickupBranch)
            .Include(x => x.ReturnBranch)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<DateTime>> GetBusyDatesByCarIdAsync(int carId)
    {
        var allRentals = await _unitOfWork.RentalDal.GetListAsync();

        var rentals = allRentals.Where(x =>
            x.CarId == carId &&
            x.Status != "İptal Edildi" &&
            x.PickupDate.HasValue &&
            x.ReturnDate.HasValue).ToList();

        var busyDates = new List<DateTime>();

        foreach (var rental in rentals)
        {
            for (var date = rental.PickupDate!.Value; date <= rental.ReturnDate!.Value; date = date.AddDays(1))
                busyDates.Add(date);
        }

        return busyDates.Distinct().ToList();
    }

    public async Task TChangeStatusAsync(int rentalId, string status, bool isApproved)
    {
        var currentRental = await _unitOfWork.Context.Rentals
            .Include(x => x.Car)
                .ThenInclude(c => c.Brand)
            .Include(x => x.Car)
                .ThenInclude(c => c.CarModel)
            .FirstOrDefaultAsync(x => x.RentalId == rentalId);

        if (currentRental != null)
        {
            //Zaten onaylanmışsa tekrar işlem yapma
            if (currentRental.Status == "Onaylandı") return;

            currentRental.Status = status;
            currentRental.IsApproved = isApproved;

            await _unitOfWork.SaveAsync();

            if (status == "Onaylandı" && isApproved)
            {
                try
                {
                    await _mailService.SendReservationApprovalMailAsync(currentRental);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MAIL ERROR: " + ex.Message);
                    throw;
                }
            }
        }
    }
    public async Task<bool> TCheckCarAvailabilityAsync(int carId, DateTime pickup, DateTime returnDate)
    {
        var car = await _unitOfWork.CarDal.GetByIdAsync(carId);

        if (car == null || !car.IsActive || !car.IsAvailable)
            return false;

        var rentals = await _unitOfWork.RentalDal.GetListAsync();

        bool isBusy = rentals.Any(r =>
            r.CarId == carId &&
            r.Status != "İptal Edildi" &&
            r.Status != "Reddedildi" &&
            r.PickupDate.HasValue && r.ReturnDate.HasValue &&
            pickup < r.ReturnDate.Value && r.PickupDate.Value < returnDate);

        return !isBusy;
    }

    public async Task<List<Rental>> TGetListWithDetailsAsync() // Bu metot, Rental'ları detaylarıyla birlikte döndürür
    {
        return await _unitOfWork.RentalDal.GetListWithDetailsAsync();
    }
}