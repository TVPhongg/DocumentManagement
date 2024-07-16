using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("ApprovalFlows")]
    public class ApprovalFlows
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public ICollection<ApprovalLevels> ApprovalLevels { get; set; }
        public ICollection<RequestDocument> RequestDocuments { get; set; }
    }
}
