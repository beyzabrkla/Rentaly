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
    public class EFCarModelDal : GenericRepository<CarModel>, ICarModelDal
    {
        public EFCarModelDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
        }
    }
}
