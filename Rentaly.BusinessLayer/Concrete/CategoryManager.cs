using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> TGetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task TInsertAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
