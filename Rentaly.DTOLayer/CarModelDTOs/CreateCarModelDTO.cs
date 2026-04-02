using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.CarModelDTOs
{
    public class CreateCarModelDTO
    {
        public string ModelName { get; set; }
        public int BrandId { get; set; }
    }
}
