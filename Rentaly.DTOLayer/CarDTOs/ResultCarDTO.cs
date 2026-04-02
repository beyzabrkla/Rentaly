
namespace Rentaly.DTOLayer.CarDTOs
{
    public class ResultCarDTO :CreateCarDTO 
    {
        public int CarId { get; set; }
        public string? BrandName { get; set; }
        public string? CarModelName { get; set; }
        public string? CategoryName { get; set; }

    }
}
