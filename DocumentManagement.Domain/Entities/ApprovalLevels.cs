using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class ApprovalLevels
    {
        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public int ApprovalFlowId { get; set; }
        public ApprovalFlows ApprovalFlows { get; set; }
        public ApprovalSteps  ApprovalStep { get; set; }
    }
}
