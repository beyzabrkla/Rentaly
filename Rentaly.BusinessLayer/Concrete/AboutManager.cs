using Rentaly.EntityLayer.Entities;

using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;

namespace Rentaly.BusinessLayer.Concrete
{
    public class AboutManager : GenericManager<About>, IAboutService
    {
        public AboutManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
