using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    internal class ApprovalFlow_DTO
    {
        public string FlowName { get; set; } 
        public List<ApprovalLevel_DTO> Levels { get; set; }
    }
}
