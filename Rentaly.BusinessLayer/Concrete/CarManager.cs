using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarManager : GenericManager<Car>, ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Car>> GetAllWithDetailsAsync() // Tüm araçları detaylı şekilde alma
        {
            return await _unitOfWork.CarDal.GetAllWithDetailsAsync();
        }

        public async Task<List<Car>> GetAvailableWithDetailsAsync() // Sadece müsait araçları detaylı şekilde alma
        {
            return await _unitOfWork.CarDal.GetAvailableWithDetailsAsync();
        }

        public async Task<Car> GetCarByIdWithDetailsAsync(int id) // CarId'ye göre detaylı araç bilgisi alma
            => await _unitOfWork.CarDal.GetCarByIdWithDetailsAsync(id);

        public async Task<List<Car>> GetCarsByBrandAsync(int brandId) // MarkaId'ye göre araçları alma
        {
            return await _unitOfWork.CarDal.GetCarsByBrandAsync(brandId);
        }

        public async Task<List<Car>> GetCarsByCategoryAsync(int categoryId) // KategoriId'ye göre araçları alma
        {
            return await _unitOfWork.CarDal.GetCarsByCategoryAsync(categoryId);
        }

        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice) // Belirli bir fiyat aralığındaki araçları alma
        {
            return await _unitOfWork.CarDal.GetCarsByPriceRangeAsync(minPrice, maxPrice);
        }

    }
}