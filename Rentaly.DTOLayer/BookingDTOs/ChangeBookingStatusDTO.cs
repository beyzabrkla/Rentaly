namespace Rentaly.DtoLayer.Dtos.BookingDTOs
{
    public class ChangeBookingStatusDTO
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public bool IsApproved { get; set; }
    }
}