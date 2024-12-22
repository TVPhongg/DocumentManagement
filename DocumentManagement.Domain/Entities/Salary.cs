using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Domain.Entities
{
    [Table("Salary")] // Đặt tên bảng để thể hiện thông tin lương
    public class Salary
    {
        [Key]
        public int Id { get; set; } // Khóa chính

        public int UserId { get; set; } // Khóa ngoại liên kết với bảng User

        // Thông tin lương
        public decimal BaseSalary { get; set; } // Lương cơ bản

        public decimal Allowances { get; set; } // Phụ cấp (ăn trưa, xăng xe...)

        public decimal Bonus { get; set; } // Tiền thưởng
     
        public decimal TotalSalary { get; set; } // Tổng lương thực nhận

        public int StandardWorkingHours { get; set; } = 176; // Giờ làm việc tiêu chuẩn trong tháng

        public int ActualWorkingHours { get; set; } // Số giờ làm việc thực tế trong tháng

        // Thông tin thời gian
        public int Year { get; set; } // Năm của kỳ lương

        public int Month { get; set; } // Tháng của kỳ lương

        public virtual Users User { get; set; }

    }
}
