namespace Rentaly.DTOLayer.RentalDTOs
{
    public class CreateRentalDTO
    {
        public int CarId { get; set; }
        public int? PickupBranchId { get; set; }
        public int? ReturnBranchId { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }

        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public decimal DailyPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DepositAmount { get; set; }

        public string? Notes { get; set; }
    }
}