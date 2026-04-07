using Rentaly.DTOLayer.CarImageDTOs;

namespace Rentaly.DTOLayer.CarDTOs
{
    public class GetCarByIdDTO: CreateCarDTO
    {
        public int CarId { get; set; }
        public string? BrandName { get; set; }
        public string? CarModelName { get; set; }
        public string? CategoryName { get; set; }
        public string? BranchName { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public new List<string> CarImages { get; set; } = new();
    }
}