using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.EntityFramework;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly RentalyContext _context;

        public UnitOfWork(RentalyContext context)
        {
            _context = context;
        }

        public IBranchDal BranchDal => new EFBranchDal(_context);
        public IBrandDal BrandDal => new EFBrandDal(_context);
        public ICarDal CarDal => new EFCarDal(_context); //Gelecekteki özel sorguların (sadece o tabloya has karmaşık metotlar) yeri hazır olsun diye CarDal'ı ekledik. Diğer tablolar için de benzer şekilde ekleyebiliriz.
        public ICarModelDal CarModelDal => new EFCarModelDal(_context);
        public ICategoryDal CategoryDal => new EFCategoryDal(_context);
        public ICustomerDal CustomerDal => new EFCustomerDal(_context);
        public IRentalDal RentalDal => new EFRentalDal(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericDal<T> GetRepository<T>() where T : class // IUnitOfWork içine generic repository eklemek için
        {
            return new GenericRepository<T>(_context); 
        }
    }
}
