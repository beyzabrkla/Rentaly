using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFBranchDal : GenericRepository<Branch>, IBranchDal
    {
        public EFBranchDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
        }
    }
}
