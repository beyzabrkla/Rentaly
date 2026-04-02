using Rentaly.DTOLayer.CarDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.BranchDTOs
{
    public class ResultBranchDTO :CreateBranchDTO
    {
        public int BranchId { get; set; }
        public List<ResultCarDTO> Cars { get; set; }
    }
}
