using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // Car'a özel metotlar
        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _unitOfWork.CarDal.GetAvailableCarsAsync();
        }

        public async Task<List<Car>> GetCarsByBrandAsync(int brandId)
        {
            return await _unitOfWork.CarDal.GetCarsByBrandAsync(brandId);
        }

        public async Task<List<Car>> GetCarsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.CarDal.GetCarsByCategoryAsync(categoryId);
        }

        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _unitOfWork.CarDal.GetCarsByPriceRangeAsync(minPrice, maxPrice);
        }
    }
}