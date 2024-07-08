using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities.Users
{
    internal class Users
    {

        [Key] 
        public int Users_id { get; set; }

        [Required] 
        [StringLength(50)] 
        public string First_name { get; set; }

        [Required]
        [StringLength(50)]
        public string Last_name { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        public string Password_hash { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int Role_id { get; set; }

        public virtual  Roles roles { get; set; }
    }
}
