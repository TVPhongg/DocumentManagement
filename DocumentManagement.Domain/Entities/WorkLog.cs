using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class WorkLog
    {
        [Key]
        public int Id { get; set; } // Khóa chính

        [Required]
        public int UserId { get; set; } // Khóa ngoại liên kết với bảng User

        [Required]
        public DateTime WorkDate { get; set; } // Tháng làm việc

        [Required]
        public int HoursWorked { get; set; } // Số giờ làm trong tháng

    }
}
