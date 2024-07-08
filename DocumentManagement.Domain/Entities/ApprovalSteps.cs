using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
 

namespace DocumentManagement.Domain.Entities
{
    public class ApprovalSteps
    {
        [Key]
        public int Id { get; set; }
        public int Request_id { get; set; }
        public int User_id { get; set; }
        public int ApprovalLevel_id { get; set; }
        public string Status { get; set; }
        public DateTime Action_date { get; set; }

        public Request_Document request {  get; set; }

        public Users Approver { get; set; }

        public ApprovalLevels approvalLevel { get; set; }
    }
}
