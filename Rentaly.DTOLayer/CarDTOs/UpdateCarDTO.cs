using Rentaly.DTOLayer.CarImageDTOs;


namespace Rentaly.DTOLayer.CarDTOs
{
    public class UpdateCarDTO :CreateCarDTO
    {
        public int CarId { get; set; }
        public string? BrandName { get; set; }
        public string? BranchName { get; set; }
        public string? CarModelName { get; set; }
        public string? CategoryName { get; set; }
        public string City { get; set; }
        public List<ResultCarImageDTO> CarImages { get; set; } = new();
    }
}
