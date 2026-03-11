using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _brandDal.DeleteAsync(id);
        }

        public async Task<Brand> TGetByIdAsync(int id)
        {
            return await _brandDal.GetByIdAsync(id);
        }

        public async Task<List<Brand>> TGetListAsync()
        {
            return await _brandDal.GetListAsync();
        }

        public async Task TInsertAsync(Brand entity)
        {
            if (string.IsNullOrWhiteSpace(entity.BrandName))
                throw new Exception("Marka adı boş olamaz");

            if (entity.BrandName.Length < 2)
                throw new Exception("Marka adı en az 2 karakter olmalıdır");

            var brands = await _brandDal.GetListAsync();

            if (brands.Any(x => x.BrandName.ToLower() == entity.BrandName.ToLower()))
                throw new Exception("Bu marka zaten mevcut");

            await _brandDal.InsertAsync(entity);
        }
        public async Task TUpdateAsync(Brand entity)
        {
            if (entity.BrandName.Length < 2)
                throw new Exception("Marka adı çok kısa");

            await _brandDal.UpdateAsync(entity);

        }
    }
}
