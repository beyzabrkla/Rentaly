using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarModelService : IGenericService<CarModel>
    {
        // CarModel'e özel metotlar
        Task<List<CarModel>> GetModelsByBrandAsync(int brandId);
    }
}