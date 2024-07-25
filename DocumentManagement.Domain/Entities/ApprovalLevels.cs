using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("ApprovalLevels")]
    public class ApprovalLevels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Step { get; set; }
        public int FlowId { get; set; }
        public int RoleId { get; set; }
        public Roles Role { get; set; }
        public ApprovalFlows ApprovalFlow { get; set; }

    }
}
