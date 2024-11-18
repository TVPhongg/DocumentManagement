using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class ApprovalLevel_DTO
    {
        public int Step { get; set; } // Bước phê duyệt 
        public int RoleId { get; set; } //Chức phụ phê duyệt ở step này 

        public string? Name { get; set; }// tên role
    }
}
