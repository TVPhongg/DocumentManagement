using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class ApprovalFlow_DTO

    {
        public int Id { get; set; } 
        public string Name { get; set; } // Tên của luồng 
        public DateTime CreatedDate { get; set; } // ngày tạo approval
        public List<ApprovalLevel_DTO> ApprovalLevels { get; set; } // list các bước phê duyệt
    }
}
