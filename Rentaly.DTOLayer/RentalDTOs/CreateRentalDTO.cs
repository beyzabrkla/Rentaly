namespace Rentaly.DTOLayer.RentalDTOs
{
    public class CreateRentalDTO
    {
        public int CarId { get; set; }
        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }

        // Müşteri Bilgileri (Formdan gelenler)
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }

        // Tarih ve Ücret
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}