using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class RentalManager : IRentalService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Rental> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Rental>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(Rental entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Rental entity)
        {
            throw new NotImplementedException();
        }
    }
}
