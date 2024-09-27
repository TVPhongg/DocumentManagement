using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocumentManagement.Domain.Entities
{
    [Table("ApprovalSteps")]
    public class ApprovalSteps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Step { get; set; }
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public DateTime UpdateTime { get; set; }
        public RequestDocument request {  get; set; }
        public Users User { get; set; }

    }
}
