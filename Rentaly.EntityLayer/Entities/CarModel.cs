using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.EntityLayer.Entities
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        public string ModelName { get; set; }
        public int BrandId { get; set; }

    }
}
