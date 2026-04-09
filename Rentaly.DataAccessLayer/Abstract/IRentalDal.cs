using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IRentalDal : IGenericDal<Rental>
    {
        Task<List<Rental>> GetListWithDetailsAsync();
    }
}