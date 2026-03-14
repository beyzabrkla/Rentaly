using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarImageManager :GenericManager<CarImage>, ICarImageService
    {
        public CarImageManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
