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

    }
}