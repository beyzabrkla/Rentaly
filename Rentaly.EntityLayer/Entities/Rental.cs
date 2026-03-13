namespace Rentaly.EntityLayer.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int PickupBranchId { get; set; }
        public virtual Branch PickupBranch { get; set; } // Alış Şubesi

        public int ReturnBranchId { get; set; }
        public virtual Branch ReturnBranch { get; set; } // İade Şubesi

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // "Aktif", "Tamamlandı", "İptal"
    }
}