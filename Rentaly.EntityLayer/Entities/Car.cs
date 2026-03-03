using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.EntityLayer.Entities
{
    public class Car
    {
        public int CarId { get; set; }
        public string PlateNumber { get; set; }
        public string VIN { get; set; } 
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
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
    }
}
