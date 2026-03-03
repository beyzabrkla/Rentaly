using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IGenericDal <T>
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task <List<T>> GetListAsync();
        Task <T> GetByIdAsync(int id);
    }
}
