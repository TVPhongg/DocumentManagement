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

        [Required]
        [StringLength(100)]
        public string Folders_name { get; set; }

        public DateTime Created_date { get; set; }

        [ForeignKey("User")]
        public int User_id { get; set; }

        [StringLength(50)]
        public string Folders_lever { get; set; }

        public virtual ICollection<Files>? File { get; set; }
        public virtual Users? User { get; set; }
    }
}
