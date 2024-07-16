using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("FolderPermissions")]
    public class FolderPermissions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FolderId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public virtual Users? User { get; set; }
        public virtual Folders? Folder { get; set; }
    }
}
