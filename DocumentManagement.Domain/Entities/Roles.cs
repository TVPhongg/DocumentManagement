using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class Roles
    {
        [Key]
        public int Role_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Roles_name { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public virtual ICollection<Users> users { get; set; }

    }
}
