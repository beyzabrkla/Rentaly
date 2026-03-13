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
    public class RentalManager : GenericManager<Rental>, IRentalService
    {
        public RentalManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
