using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class View_ApprovalStep
    {
        [Key]
        public int Id { get; set; }
        public int Request_id { get; set; }
        public int User_id { get; set; }
        public int ApprovalLevel_id { get; set; }
        public string Status { get; set; }
        public DateTime Action_date { get; set; }
    }
}
