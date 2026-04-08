namespace Rentaly.DTOLayer.RentalDTOs
{
    public class CreateRentalDTO
    {
        public int CarId { get; set; }
        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }

        // Müşteri Bilgileri
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }

        // OCR İçin Eklenenler
        public DateTime? BirthDate { get; set; } // Yaş kontrolü için
        public string? IdCardImageUrl { get; set; } // Yüklenen kimliğin yolu

        // Tarih ve Ücret
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal DailyPrice { get; set; } // O anki fiyatı sabitlemek için
        public decimal TotalPrice { get; set; }
        public decimal DepositAmount { get; set; } // Depozito bilgisi

        // Notlar (Müşterinin özel istekleri için)
        public string? Notes { get; set; }
    }
}