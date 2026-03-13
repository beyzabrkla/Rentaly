using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFCarDal : GenericRepository<Car>, ICarDal
    {
        public EFCarDal(RentalyContext rentalyContext) : base(rentalyContext)
        {
        }
    }
}
