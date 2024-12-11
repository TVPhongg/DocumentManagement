using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("Projects ")]
    public class Projects
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName {  get; set; }
        public int Status { get; set; } //(Pending, In Progress, Completed)
        public int Priority { get; set; }//(Low, Medium, High)
        public string? Description {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CreateBy { get; set; }
        public List<int>? TeamMember { get; set; }

    }
}
