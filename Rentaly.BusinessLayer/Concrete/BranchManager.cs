using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BranchManager : GenericManager<Branch>, IBranchService
    {
        public BranchManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}