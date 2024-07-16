﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("RequestDocument")]
    public class RequestDocument
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string File {  get; set; }
        public int UserId { get; set; }
        public int  FlowId { get; set; }
        public ICollection<ApprovalSteps> ApprovalSteps { get; set;}
        public Users User { get; set; }
        public ApprovalFlows ApprovalFlow { get; set; }
    }
}
