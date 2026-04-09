using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IRentalService : IGenericService<Rental>
    {
        // Admin onayı için statü değiştirme
        Task TChangeStatusAsync(int rentalId, string status, bool isApproved);

        // Müsaitlik kontrolü (Senin Case: "Seçilen tarih aralığında kilitlenecek")
        Task<bool> TCheckCarAvailabilityAsync(int carId, DateTime pickup, DateTime returnDate);
        Task<List<DateTime>> GetBusyDatesByCarIdAsync(int carId); // JS tarafında tarihleri kapatmak için
        Task<List<Rental>> TGetListWithDetailsAsync(); // Kiralamaları detaylarıyla birlikte getirmek için
        Task<List<Rental>> TGetRentalsWithDetailsAsync();// Kiralamaları detaylarıyla birlikte getirmek için (Admin tarafında)
    }
}
