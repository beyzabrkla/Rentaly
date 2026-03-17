using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentaly.EntityLayer.Entities
{
    public class CarImage
    {
        [Key]
        public int CarImageId { get; set; }

        public string? CoverImageUrl { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car? Car { get; set; }
    }
}