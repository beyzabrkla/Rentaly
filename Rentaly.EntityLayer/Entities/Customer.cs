using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.EntityLayer.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdentityNumber { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }
    }
}
