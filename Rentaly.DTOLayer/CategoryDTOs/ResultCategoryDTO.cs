using Rentaly.DTOLayer.CarDTOs;

namespace Rentaly.DTOLayer.CategoryDTOs
{
    public class ResultCategoryDTO :CreateCategoryDTO
    {
        public int CategoryId { get; set; }
        public List<ResultCarDTO> Cars { get; set; }

    }
}
