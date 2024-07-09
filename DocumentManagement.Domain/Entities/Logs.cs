using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Entities
{
    [Table("Logs")]
    public class Logs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Foleders_id")]
        public int Foleders_id { get; set; }
        [ForeignKey("File_id")]
        public int File_id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Activity { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime Created_date { get; set; }
        [ForeignKey("User_id")]
        public int User_id { get; set; }
        [ForeignKey("Request_id")]
        public int Request_id { get; set; }
        [ForeignKey("ApprovalSteps_id")]
        public int ApprovalSteps_id { get; set; }

    }
}
