namespace Rentaly.DTOLayer.CustomerDTOs
{
    public class CreateCustomerDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? DrivingLicenseDate { get; set; }
    }
}