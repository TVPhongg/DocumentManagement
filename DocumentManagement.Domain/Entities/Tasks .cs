using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        public string TaskName { get; set; }

        public string Description { get; set; }

        public int AssignedTo { get; set; }//người được giao cv

        public int CreatedBy { get; set; }//người tạo công việc

        public DateTime CreatedAt { get; set; }// thời gian tạo cv

        public DateTime DueDate { get; set; }//hạn hoàn thành cv 

        public int  Status { get; set; }// trạng thái cv

        public int Priority {  get; set; }// Mức đọ ưu tiên

        public int Progress { get; set; }// tiến độ công việc

        public Users User { get; set; }

        //public ICollection<TaskUpdates> TaskUpdates { get; set; }
    }
}
