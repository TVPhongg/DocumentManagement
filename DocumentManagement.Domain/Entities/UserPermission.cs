using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("UserPermission")]
    public class UserPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PermissionId { get; set; }
        public int UserId { get; set; }
        public string ActionName { get; set; }
        public string CheckAction { get; set; }
        public virtual Permission? Permission { get; set; }
        public virtual Users? User { get; set; }
    }
}
