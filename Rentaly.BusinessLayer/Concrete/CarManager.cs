using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarManager : ICarService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Car> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Car>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(Car entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Car entity)
        {
            throw new NotImplementedException();
        }
    }
}
