using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BrandManager : GenericManager<Brand>, IBrandService
    {
        private readonly IUnitOfWork _uow;

        public BrandManager(IUnitOfWork uow) :base(uow) 
        {
            _uow = uow;
        }

        public override async Task TInsertAsync(Brand entity)
        {
            var brands = await _uow.BrandDal.GetListAsync();
            if (brands.Any(x => !string.IsNullOrEmpty(x.BrandName) &&
                                x.BrandName.ToLower() == entity.BrandName.ToLower()))
                throw new Exception("Bu marka zaten mevcut");

            await base.TInsertAsync(entity);
        }
        public override async Task TUpdateAsync(Brand entity)
        {
            await base.TUpdateAsync(entity);
        }
    }
}
