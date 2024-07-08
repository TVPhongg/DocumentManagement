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
    internal class Files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Foleders_id")]
        public int Foleders_id { get; set; }
        public string File_name { get; set; }
        public string File_path { get; set; }
        public DateTime Created_date { get; set; }
        [ForeignKey("User_id")]
        public int User_id { get; set; }
        public float File_size { get; set; }

        public virtual Foleders? Foleders { get; set; }
    }
}
