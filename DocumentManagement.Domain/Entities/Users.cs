using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Roles")]
        public int? RoleId { get; set; }

        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }

        public virtual Roles Role { get; set; }

        public Department Department { get; set; }

        public virtual Salary Salary { get; set; }

        public ICollection<RequestDocument> RequestDocuments { get; set; }

        public ICollection<ApprovalSteps> ApprovalSteps { get; set; }

        public ICollection<Tasks> Tasks { get; set; }

        //public ICollection<TaskUpdates> TaskUpdates { get; set; }
    }
}
