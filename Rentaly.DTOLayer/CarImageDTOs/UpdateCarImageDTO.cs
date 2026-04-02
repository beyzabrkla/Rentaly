using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.CarImageDTOs
{
    public class UpdateCarImageDTO :CreateCarImageDTO
    {
        public int CarImageId { get; set; }
    }
}
