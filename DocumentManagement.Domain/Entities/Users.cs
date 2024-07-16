using DocumentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Users
{

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }

        public virtual Roles Role { get; set; }
        public ICollection<RequestDocument> RequestDocuments{ get; set; }
        public ICollection<ApprovalSteps> ApprovalSteps { get; set; }
        public ICollection<FolderPermissions> FolderPermissions { get; set; }
        public ICollection<FilePermissions> FilePermissions { get; set; } 


}
