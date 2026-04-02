using Rentaly.DTOLayer.CarDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.CategoryDTOs
{
    public class UpdateCategoryDTO:CreateCategoryDTO
    {
        public int CategoryId { get; set; }
    }
}
