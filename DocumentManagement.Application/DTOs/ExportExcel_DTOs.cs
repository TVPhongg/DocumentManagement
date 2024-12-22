using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class ExportExcel_DTOs
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public string Gender { get; set; }

        public decimal BaseSalary { get; set; } // Lương cơ bản

        public decimal Allowances { get; set; } // Phụ cấp (ăn trưa, xăng xe...)

        public decimal Bonus { get; set; } // Tiền thưởng

        public decimal TotalSalary { get; set; }

        public int ActualWorkingHours { get; set; }// Giờ làm việc trong tháng
        public int Year { get; set; } // Năm của kỳ lương

        public int Month { get; set; } // Tháng của kỳ lương
    }
}
