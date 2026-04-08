using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFCarDal : GenericRepository<Car>, ICarDal
    {
        private readonly RentalyContext _context;

        public EFCarDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        // Tüm navigation property'leri içeren merkezi sorgu
        private IQueryable<Car> WithAllIncludes()
        {
            return _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Category)
                .Include(c => c.Branch)
                .Include(c => c.CarImages);
        }

        /// Admin: Tüm araçlar, filtre yok
        public async Task<List<Car>> GetAllWithDetailsAsync()
            => await WithAllIncludes().ToListAsync();

        /// Müşteri: sadece müsait+aktif araçlar
        public async Task<List<Car>> GetAvailableWithDetailsAsync()
            => await WithAllIncludes()
                .Where(c => c.IsAvailable && c.IsActive)
                .ToListAsync();

        //Markaya göre araçlar
        public async Task<List<Car>> GetCarsByBrandAsync(int brandId)
            => await WithAllIncludes()
                .Where(c => c.BrandId == brandId)
                .ToListAsync();

        // Modeline göre araçlar
        public async Task<List<Car>> GetCarsByCategoryAsync(int categoryId)
            => await WithAllIncludes()
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();

        // Fiyat aralığına göre araçlar
        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
            => await WithAllIncludes()
                .Where(c => c.DailyPrice >= minPrice && c.DailyPrice <= maxPrice)
                .ToListAsync();


        //Araç detayları (ID'ye göre)
        public async Task<Car> GetCarByIdWithDetailsAsync(int id)
        {
            return await WithAllIncludes()
                .FirstOrDefaultAsync(c => c.CarId == id);
        }

        // Şubeye göre araçlar (detaylı)
        public async Task<List<Car>> GetCarsByBranchWithDetailsAsync(int branchId)
            => await WithAllIncludes()
                    .Where(x => x.BranchId == branchId)
                    .ToListAsync();

        public async Task<List<Car>> GetAvailableCarsByDateAsync(DateTime pickupDate, DateTime returnDate, int? branchId)
        {
            // O tarihlerde meşgul olan araç ID'lerini bulalım
            var busyCarIds = await _context.Rentals
                .Where(r => (r.Status == "Onaylandı" || r.Status == "Beklemede") &&
                            pickupDate < r.ReturnDate && r.PickupDate < returnDate)
                .Select(r => r.CarId)
                .ToListAsync();

            //Bu ID'ler dışındaki ve şubeye uygun araçları getirelim
            var query = WithAllIncludes()
                .Where(c => c.IsActive && !busyCarIds.Contains(c.CarId));

            if (branchId.HasValue)
            {
                query = query.Where(c => c.BranchId == branchId.Value);
            }

            return await query.ToListAsync();
        }
    }
}