using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class UpdateSalary_DTOs
    {
        public int UserId { get; set; } // Bắt buộc
        public decimal BaseSalary { get; set; } // Bắt buộc
        public decimal Allowances { get; set; } // Bắt buộc

        public decimal Bonus { get; set; } // Bắt buộc

        public int StandardWorkingHours { get; set; } = 176;

        public WorkLog_DTOs? WorkLog { get; set; } // Chỉ chứa thông tin cần cho WorkLog
    }
    
}
