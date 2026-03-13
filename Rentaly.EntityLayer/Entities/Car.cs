using System.ComponentModel.DataAnnotations;

namespace Rentaly.EntityLayer.Entities
{
    public class Car
    {
        public int CarId { get; set; }

        [Required, StringLength(15)]
        public string PlateNumber { get; set; } // Plaka

        [Required, StringLength(50)]
        public string VIN { get; set; } // Şase No

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } // Navigation

        public int CarModelId { get; set; } // İsim tutarlılığı sağlandı
        public virtual CarModel CarModel { get; set; } // Navigation

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } // Navigation

        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; } // Navigation

        public int Year { get; set; }
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public int PersonCount { get; set; }
        public int SeatCount { get; set; }
        public int LuggageCount { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; } // Şanzıman eklendi (Manuel/Otomatik)
    }
}