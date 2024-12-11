﻿using System;
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

        public string? Description {  get; set; }

        public int AssignedTo{ get; set; }

        public int ProjectId {  get; set; }

        public int Status { get; set; } //(Pending, In Progress, Completed)

        public int Priority {  get; set; }//(Low, Medium, High)

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
