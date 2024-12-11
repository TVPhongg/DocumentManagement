using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class RegisterUserDto
    {
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
        public int RoleId { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
    }
}
