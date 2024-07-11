using DocumentManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public int Users_id { get; set; }
    [Required]
    [StringLength(50)]
    public string First_name { get; set; }
    [Required]
    [StringLength(50)]
    public string Last_name { get; set; }
    [StringLength(100)]
    public string Address { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(10)]
    public string Gender { get; set; }
    [Required]
    public string Password_hash { get; set; }

    [Required]
    [ForeignKey("Role")]
    public int Role_id { get; set; }
    public Roles? Role { get; set; }
    public virtual ICollection<Folders> Folder { get; set; }
    public virtual ICollection<Files> File { get; set; }
}
