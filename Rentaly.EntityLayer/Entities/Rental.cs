namespace Rentaly.EntityLayer.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }

        // Araç İlişkisi
        public int CarId { get; set; }
        public virtual Car? Car { get; set; }

        // Müşteri Bilgileri (Case gereği doğrudan burada tutuyoruz)
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }

        // Şube İlişkileri (Case'de Alış ve Dönüş lokasyonu var)
        public int PickupBranchId { get; set; }
        public virtual Branch? PickupBranch { get; set; }

        public int ReturnBranchId { get; set; }
        public virtual Branch? ReturnBranch { get; set; }

        // Tarih ve Ücret
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Admin Yönetim Alanları (Case'deki Onay Mekanizması)
        public bool IsApproved { get; set; } = false;
        public string? Status { get; set; } = "Beklemede"; // Beklemede, Onaylandı, İptal Edildi, Tamamlandı
    }
}