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
            var value = _rentalyContext.Set<T>().Find(id);
            _rentalyContext.Set<T>().Remove(value);
            await _rentalyContext.SaveChangesAsync();
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
            await _rentalyContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _rentalyContext.Set<T>().Update(entity);
            return _rentalyContext.SaveChangesAsync();
        }
    }
}
