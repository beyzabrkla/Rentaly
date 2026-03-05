using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFBranchDal : GenericRepository<Branch>, IBranchDal
    {
        public EFBranchDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
        }
    }
}
