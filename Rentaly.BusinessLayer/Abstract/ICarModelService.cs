using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarModelService : IGenericService<CarModel>
    {
        // CarModel'e özel metotlar
        Task<List<CarModel>> GetModelsByBrandAsync(int brandId);
        Task<List<CarModel>> GetCarModelsWithBrandsAsync();
    }
}