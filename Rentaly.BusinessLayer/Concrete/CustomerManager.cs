using Microsoft.EntityFrameworkCore;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CustomerManager : GenericManager<Customer>, ICustomerService
    {
        private readonly IUnitOfWork _uow;

        public CustomerManager(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public async Task<Customer> TGetByIdentityNumberAsync(string identityNumber)
        {
            return await _uow.Context.Customers
                .FirstOrDefaultAsync(x => x.IdentityNumber == identityNumber);
        }
    }
}