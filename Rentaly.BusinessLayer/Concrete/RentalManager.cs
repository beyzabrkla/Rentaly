using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

public class RentalManager : GenericManager<Rental>, IRentalService
{
    private readonly IUnitOfWork _unitOfWork;

    public RentalManager(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DateTime>> GetBusyDatesByCarIdAsync(int carId)
    {
        var allRentals = await _unitOfWork.RentalDal.GetListAsync();

        //Bellek üzerinde (In-Memory) filtreleme yapıyoruz
        // ÖNEMLİ: Status kontrolünü veritabanındaki karşılığına göre (İptal Edildi veya Cancelled) güncelle
        var rentals = allRentals.Where(x => x.CarId == carId && x.Status != "İptal Edildi").ToList();

        var busyDates = new List<DateTime>();

        foreach (var rental in rentals)
        {
            // Alış ve İade tarihleri arasındaki her günü listeye ekliyoruz
            for (var date = rental.PickupDate; date <= rental.ReturnDate; date = date.AddDays(1))
            {
                busyDates.Add(date);
            }
        }

        return busyDates.Distinct().ToList();
    }

    public async Task TChangeStatusAsync(int rentalId, string status, bool isApproved)
    {
        var rental = await _unitOfWork.RentalDal.GetByIdAsync(rentalId);
        if (rental != null)
        {
            rental.Status = status;
            rental.IsApproved = isApproved;
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task<bool> TCheckCarAvailabilityAsync(int carId, DateTime pickup, DateTime returnDate)
    {
        var car = await _unitOfWork.CarDal.GetByIdAsync(carId);

        // Eğer admin aracı pasife aldıysa veya manuel olarak "Müsait Değil" dediyse takvime bakmaya gerek yok.
        if (car == null || !car.IsActive || !car.IsAvailable)
        {
            return false;
        }

        //Takvim çakışması kontrolü
        var rentals = await _unitOfWork.RentalDal.GetListAsync();

        bool isBusy = rentals.Any(r =>
            r.CarId == carId &&
            r.Status != "İptal Edildi" && // Sadece iptal edilmeyenler meşgul sayılır
            r.Status != "Reddedildi" &&    // Eğer böyle bir statün varsa ekle
            pickup < r.ReturnDate && r.PickupDate < returnDate);

        return !isBusy;
    }
}