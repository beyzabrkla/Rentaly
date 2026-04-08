namespace Rentaly.DtoLayer.Dtos.BookingDtos
{
    public class CreateBookingDTO
    {
        public int CarId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdentityNumber { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public string PickUpLocation { get; set; }
        public decimal TotalPrice { get; set; }
    }
}