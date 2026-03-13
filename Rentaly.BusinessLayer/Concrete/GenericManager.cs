 using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;

namespace Rentaly.BusinessLayer.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork _uow;

        public GenericManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public virtual async Task TInsertAsync(T entity)
        {
            await _uow.GetRepository<T>().InsertAsync(entity);
            await _uow.SaveAsync();
        }

        public async Task TDeleteAsync(int id)
        {
            await _uow.GetRepository<T>().DeleteAsync(id);
            await _uow.SaveAsync();
        }

        public virtual async Task TUpdateAsync(T entity) //virtual olarak işaretledik çünkü gerektiğinde alt sınıflarda override edilebilir
        {
            await _uow.GetRepository<T>().UpdateAsync(entity);
            await _uow.SaveAsync();
        }

        public async Task<List<T>> TGetListAsync()
        {
            return await _uow.GetRepository<T>().GetListAsync();
        }

        public async Task<T> TGetByIdAsync(int id)
        {
            return await _uow.GetRepository<T>().GetByIdAsync(id);
        }
    }
}