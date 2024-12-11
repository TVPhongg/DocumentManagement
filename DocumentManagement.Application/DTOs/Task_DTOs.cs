using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Task_DTOs
    {
        public int Id { get; set; }

        public string TaskName { get; set; }

        public string? Description { get; set; }

        public int AssignedTo { get; set; }

        public int ProjectId { get; set; }

        public int Status { get; set; } //(Pending, In Progress, Completed)

        public int Priority { get; set; }//(Low, Medium, High)

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? FullName { get; set; }

        public string? AssignedToEmail { get; set; }
    }
}
