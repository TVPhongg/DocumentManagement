using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("Folders")]
    public class Folders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual ICollection<Files>? File { get; set; }
        public ICollection<FolderPermissions> FolderPermissions { get; set; }
    }
}
