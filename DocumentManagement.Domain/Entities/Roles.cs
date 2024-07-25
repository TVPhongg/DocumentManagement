using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<ApprovalLevels> ApprovalLevels{ get; set; }

    }
}
