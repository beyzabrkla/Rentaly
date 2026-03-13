using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarService : IGenericService<Car>
    {
        // Car'a özel business metotları
        Task<List<Car>> GetAvailableCarsAsync();
        Task<List<Car>> GetCarsByBrandAsync(int brandId);
        Task<List<Car>> GetCarsByCategoryAsync(int categoryId);
        Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}