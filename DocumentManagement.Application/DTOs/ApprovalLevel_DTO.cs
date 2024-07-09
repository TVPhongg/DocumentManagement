using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    internal class ApprovalLevel_DTO
    {
        public int Level { get; set; }  // Số thứ tự của cấp phê duyệt
        public int ApproverId { get; set; }  // ID của người phê duyệt
    }
}
