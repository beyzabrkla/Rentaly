using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task TInsertAsync(T entity);
        Task TDeleteAsync(int id);
        Task TUpdateAsync(T entity);
        Task <List<T>> TGetListAsync();
        Task<T> TGetByIdAsync(int id);
    }
}
