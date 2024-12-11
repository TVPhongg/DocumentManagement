using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Project_DTOs
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public int Status { get; set; } //(Pending, In Progress, Completed)

        public int Priority { get; set; }//(Low, Medium, High)

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreateBy { get; set; }

        public string? CreatedByName {  get; set; }

        public List<int>? TeamMember { get; set; }
        
        public int TeamSize { get; set; }

        public int ProjectCount { get; set; }

        public int ProjectOpenCount { get; set; }

        public int ProjectInProgressCount { get; set; }

        public int ProjectDoneCount { get; set; }
        public List<UserDto>? MembersDetail { get; set; }
    }
}
