using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IGenericDal <T>
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task <List<T>> GetListAsync();
        Task <T> GetByIdAsync(int id);
        Task<List<T>> GetListWithIncludeAsync(params Expression<Func<T, object>>[] includes);
    }
}
