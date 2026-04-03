using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.BrandDTOs
{
    public class CreateBrandDTO
    {
        public string BrandName { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool Status { get; set; } = true;
    }
}
