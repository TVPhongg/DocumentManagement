using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Log_DTOs
    {
        public int Id { get; set; }
        public int Foleders_id { get; set; }
        public int File_id { get; set; }
        public string Activity { get; set; }
        public DateTime Created_date { get; set; }
        public int User_id { get; set; }
        public int Request_id { get; set; }
        public int ApprovalSteps_id { get; set; }
    }
}
