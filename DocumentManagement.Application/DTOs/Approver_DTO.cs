using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Approver_DTO
    {
        public int Step { get; set; } // Bước phê duyệt 
        public int UserId { get; set; } // id của ng phê duyệt trong tùng step
        public string NameUser { get; set; } // Tên người phê duyệt trong từng step 
        public string RoleName { get; set; } // Tên role của từng step phê duyệt trong luồng phê duyệt 
    }
}
