namespace Rentaly.DTOLayer.RentalDTOs
{
    public class ResultRentalDTO : CreateRentalDTO
    {
        public int RentalId { get; set; }
        public string? Status { get; set; }
        public bool IsApproved { get; set; }

        // UI'da güzel görünmesi için ek bilgiler
        public string? CarPlate { get; set; }
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
    }
}