using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DTOLayer.BranchDTOs
{
    public class CreateBranchDTO
    {
        public string BranchName { get; set; }
        public string City { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
