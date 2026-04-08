using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFProcessDal : GenericRepository<Process>, IProcessDal
    {
        public EFProcessDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
        }
    }
}
