using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class Request_Document
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Created_date { get; set; }
        public string Status {  get; set; }
        public ICollection<ApprovalSteps> ApprovalStep { get; set; }
    }
}
