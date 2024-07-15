using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
 

namespace DocumentManagement.Domain.Entities
{
    [Table("ApprovalSteps")]
    public class ApprovalSteps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Step { get; set; }
        public int UserId { get; set; }
        public int RequestId { get; set; }        
        public int Status { get; set; }
        public DateTime UpdateTime { get; set; }

        public RequestDocument request {  get; set; }

        public Users User { get; set; }

    }
}
