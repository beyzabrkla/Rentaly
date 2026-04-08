namespace Rentaly.DTOLayer.RentalDTOs
{
    public class ChangeRentalStatusDTO
    {
        public int RentalId { get; set; }
        public string? Status { get; set; }
        public bool IsApproved { get; set; }
    }
}