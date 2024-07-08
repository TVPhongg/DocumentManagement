using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class View_ApprovalLevel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int ApprovalFlowId { get; set; }
    }
}
