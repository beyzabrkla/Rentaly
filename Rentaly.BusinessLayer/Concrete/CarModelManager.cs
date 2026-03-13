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
    public class CarModelManager : GenericManager<CarModel>, ICarModelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarModelManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CarModel>> GetModelsByBrandAsync(int brandId)
        {
            return await _unitOfWork.CarModelDal.GetModelsByBrandAsync(brandId);
        }
    }
}