using System.ComponentModel.DataAnnotations;

namespace Rentaly.EntityLayer.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }

        [Required, StringLength(11)]
        public string IdentityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime DrivingLicenseDate { get; set; }
    }
}