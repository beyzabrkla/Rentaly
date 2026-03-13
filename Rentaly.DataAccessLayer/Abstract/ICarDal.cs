using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarDal : IGenericDal<Car>
    {
        // Car'a özel kompleks sorgular
        Task<List<Car>> GetAvailableCarsAsync();
        Task<List<Car>> GetCarsByBrandAsync(int brandId);
        Task<List<Car>> GetCarsByCategoryAsync(int categoryId);
        Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}