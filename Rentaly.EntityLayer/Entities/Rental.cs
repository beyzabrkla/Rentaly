namespace Rentaly.EntityLayer.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public virtual Car? Car { get; set; }

        // Müşteri Bilgileri
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }

        // Şube İlişkileri
        public int PickupBranchId { get; set; }
        public virtual Branch? PickupBranch { get; set; }

        public int ReturnBranchId { get; set; }
        public virtual Branch? ReturnBranch { get; set; }

        // Tarih ve Ücret Detayları
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal TotalPrice { get; set; }

        // DTO'da olup burada eksik kalan alan
        public string? Notes { get; set; }

        // Zaman Takibi
        public DateTime CreatedDate { get; set; }

        // Yönetim Alanları
        public bool IsApproved { get; set; } = false;
        public string? Status { get; set; } = "Beklemede";

        // Admin'in neden iptal ettiğini tutmak istersen (DTO'da var, buraya da ekledik)
        public string? AdminNote { get; set; }
    }
}