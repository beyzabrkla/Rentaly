using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CustomerManager : ICustomerService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
