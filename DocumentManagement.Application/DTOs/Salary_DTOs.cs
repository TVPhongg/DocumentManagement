using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Domain.Entities;

namespace DocumentManagement.Application.DTOs
{
    public class Salary_DTOs
    {
        public int Id { get; set; } // Khóa chính

        public int UserId { get; set; } // Khóa ngoại liên kết với bảng User

        // Thông tin lương
        public decimal BaseSalary { get; set; } // Lương cơ bản

        public decimal Allowances { get; set; } // Phụ cấp (ăn trưa, xăng xe...)

        public decimal Bonus { get; set; } // Tiền thưởng

        public decimal TotalSalary {  get; set; }

        public int StandardWorkingHours { get; set; } = 176; // Giờ làm việc tiêu chuẩn trong tháng

        public int ActualWorkingHours { get; set; } // Số giờ làm việc thực tế trong tháng

        // Thông tin thời gian
        public int Year { get; set; } // Năm của kỳ lương

        public int Month { get; set; } // Tháng của kỳ lương

        public WorkLog? WorkLog { get; set; }
    }
}
