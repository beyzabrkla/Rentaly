using Rentaly.DtoLayer.Dtos.BookingDtos;

public class ResultBookingDto : CreateBookingDTO
{
    public int BookingId { get; set; }
    public string Status { get; set; }
    public bool IsApproved { get; set; }
    public string CarModel { get; set; }
}