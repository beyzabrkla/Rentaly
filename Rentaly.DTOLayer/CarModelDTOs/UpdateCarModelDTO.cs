using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.CarModelDTOs
{
    public class UpdateCarModelDTO:CreateCarModelDTO
    {
        public int CarModelId { get; set; }
        public string BrandName { get; set; }

    }
}
