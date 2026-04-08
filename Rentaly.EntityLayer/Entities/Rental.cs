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
        public DateTime DrivingLicenseDate { get; set; }

        // OCR ve Güvenlik İçin Eklenenler
        public DateTime? BirthDate { get; set; } // Yaş kontrolü için
        public string? IdCardImageUrl { get; set; } // Kimlik fotokopisi kanıtı

        // Şube İlişkileri
        public int PickupBranchId { get; set; }
        public virtual Branch? PickupBranch { get; set; }

        public int ReturnBranchId { get; set; }
        public virtual Branch? ReturnBranch { get; set; }

        // Tarih ve Ücret Detayları
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal DailyPrice { get; set; } // Kiralama anındaki günlük fiyat
        public decimal DepositAmount { get; set; } // Kiralama anındaki depozito
        public decimal TotalPrice { get; set; }

        // Zaman Takibi
        public DateTime CreatedDate { get; set; }

        // Yönetim Alanları
        public bool IsApproved { get; set; } = false;
        public string? Status { get; set; } = "Beklemede";

        // Operasyonel Notlar
        public string? AdminNote { get; set; } // Reddedilirse neden reddedildi?
    }
}