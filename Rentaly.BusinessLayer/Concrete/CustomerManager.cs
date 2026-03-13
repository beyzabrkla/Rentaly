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
    public class CustomerManager : GenericManager<Customer>, ICustomerService
    {
        public CustomerManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
