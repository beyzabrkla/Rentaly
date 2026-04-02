using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.RentalDTOs
{
    public class CreateRentalDTO
    {
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }

    }
}
