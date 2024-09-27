using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("Files")]
    public class Files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("FoldersId")]
        public int FoldersId { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public decimal FileSize { get; set; }

        public virtual Folders? Folder { get; set; }
        public ICollection<FilePermissions> FilePermissions { get; set; }
    }
}
