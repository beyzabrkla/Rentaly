using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFRentalDal : GenericRepository<Rental>, IRentalDal
    {
        private readonly RentalyContext _context;

        public EFRentalDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetListWithDetailsAsync()
        {
            return await _context.Rentals
                .Include(x => x.Car)
                    .ThenInclude(c => c.Brand)
                .Include(x => x.Car)
                    .ThenInclude(c => c.CarModel)
                .Include(x => x.PickupBranch)
                .Include(x => x.ReturnBranch)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}