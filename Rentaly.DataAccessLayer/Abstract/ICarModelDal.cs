using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarModelDal : IGenericDal<CarModel>
    {
        // CarModel'e özel metotlar
        Task<List<CarModel>> GetModelsByBrandAsync(int brandId);
        Task<List<CarModel>> GetCarModelsWithBrands();
    }
}