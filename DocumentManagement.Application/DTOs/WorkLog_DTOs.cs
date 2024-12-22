using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class WorkLog_DTOs
    {
        public int Id { get; set; } // Khóa chính

        public int UserId { get; set; } // Khóa ngoại liên kết với bảng User

        public DateTime WorkDate { get; set; } // Ngày làm việc

        public int HoursWorked { get; set; } // Số giờ làm trong ngày

    }
}
