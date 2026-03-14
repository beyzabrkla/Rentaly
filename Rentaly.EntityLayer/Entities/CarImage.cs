using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.EntityLayer.Entities
{
    public class CarImage
    {
        [Key]
        public int CarImageId { get; set; }

        [Required]
        public string CoverImageUrl { get; set; }

        public int CarId { get; set; }

        [ForeignKey("CarId")] 
        public virtual Car Car { get; set; }
    }
}
