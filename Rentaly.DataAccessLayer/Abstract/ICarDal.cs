using Rentaly.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarDal : IGenericDal<Car>
    {
        Task<List<Car>> GetCarsByBrandAsync(int brandId);
        Task<List<Car>> GetCarsByCategoryAsync(int categoryId);
        Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<List<Car>> GetAllWithDetailsAsync();
        Task<List<Car>> GetAvailableWithDetailsAsync();
        Task<Car> GetCarByIdWithDetailsAsync(int id);
    }
}