using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EFCarModelDal : GenericRepository<CarModel>, ICarModelDal
    {
        private readonly RentalyContext _context;

        public EFCarModelDal(RentalyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CarModel>> GetModelsByBrandAsync(int brandId)
        {
            return await _context.CarModels
                .Where(m => m.BrandId == brandId)
                .Include(m => m.Brand)
                .ToListAsync();
        }
    }
}