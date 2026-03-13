using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;

namespace Rentaly.DataAccessLayer.RepositoryDesignPattern
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly RentalyContext _rentalyContext;

        public GenericRepository(RentalyContext rentalyContext)
        {
            _rentalyContext = rentalyContext;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _rentalyContext.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _rentalyContext.Set<T>().Remove(entity);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _rentalyContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetListAsync()
        {
            return await _rentalyContext.Set<T>().ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _rentalyContext.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _rentalyContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
