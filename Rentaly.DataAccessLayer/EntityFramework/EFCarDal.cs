using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFCarDal : GenericRepository<Car>, ICarDal
    {
        private readonly RentalyContext _rentalyContext;

        public EFCarDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
            _rentalyContext = rentalyContext;
        }

        // Müsait araçları getir (IsAvailable = true ve IsActive = true)
        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _rentalyContext.Cars
                .Where(c => c.IsAvailable && c.IsActive)
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Category)
                .Include(c => c.Branch)
                .ToListAsync();
        }

        // Markaya göre araçları getir
        public async Task<List<Car>> GetCarsByBrandAsync(int brandId)
        {
            return await _rentalyContext.Cars
                .Where(c => c.BrandId == brandId && c.IsActive)
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Category)
                .Include(c => c.Branch)
                .ToListAsync();
        }

        // Kategoriye göre araçları getir
        public async Task<List<Car>> GetCarsByCategoryAsync(int categoryId)
        {
            return await _rentalyContext.Cars
                .Where(c => c.CategoryId == categoryId && c.IsActive)
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Category)
                .Include(c => c.Branch)
                .ToListAsync();
        }

        // Fiyat aralığına göre araçları getir
        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _rentalyContext.Cars
                .Where(c => c.DailyPrice >= minPrice && c.DailyPrice <= maxPrice && c.IsActive)
                .Include(c => c.Brand)
                .Include(c => c.CarModel)
                .Include(c => c.Category)
                .Include(c => c.Branch)
                .OrderBy(c => c.DailyPrice)
                .ToListAsync();
        }
    }
}