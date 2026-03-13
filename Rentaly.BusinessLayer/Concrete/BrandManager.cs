using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // 1. İş akışı kontrolü (Örn: Veritabanında aynı isim var mı?)
            var brands = await _uow.BrandDal.GetListAsync();
            if (brands.Any(x => x.BrandName.ToLower() == entity.BrandName.ToLower()))
                throw new Exception("Bu marka zaten mevcut");

            // 2. Kayıt işlemi
            await base.TInsertAsync(entity);
        }
        public override async Task TUpdateAsync(Brand entity) // 'override' ekledik
        {
            if (entity.BrandName.Length < 2)
                throw new Exception("Marka adı çok kısa");

            await base.TUpdateAsync(entity); // base üzerinden güncelleme yapıyoruz
        }
    }
}
