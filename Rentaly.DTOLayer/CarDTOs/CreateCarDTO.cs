using Rentaly.DTOLayer.CarImageDTOs;

namespace Rentaly.DTOLayer.CarDTOs
{
    public class CreateCarDTO
    {
        public string PlateNumber { get; set; }
        public string VIN { get; set; }
        public int BrandId { get; set; }
        public int CarModelId { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public List<ResultCarImageDTO> CarImages { get; set; } = new List<ResultCarImageDTO>();
        public string? CoverImageUrl { get; set; }
        public int PersonCount { get; set; }
        public int SeatCount { get; set; }
        public int LuggageCount { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }

    }
}
