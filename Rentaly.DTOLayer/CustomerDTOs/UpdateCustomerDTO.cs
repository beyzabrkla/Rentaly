using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.CustomerDTOs
{
    public class UpdateCustomerDTO:CreateCustomerDTO
    {
        public int CustomerId { get; set; }

    }
}
