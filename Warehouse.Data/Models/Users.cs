using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Models;

public class Users
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int UserID { get; set; }
  public int? RoleID { get; set; }
  [Required]
  [StringLength(100)]
  public string Name { get; set; }
  [Required]
  [StringLength(100)]
  public string Surname { get; set; }
  [EmailAddress]
  [Required]
  public string Email { get; set; }
  [Required]
  [StringLength(255)] // Adjust length as needed
  public string PasswordHash { get; set; }
  [DataType(DataType.Date)]
  public DateTime BirthDate { get; set; }
  public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
  public ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();
  public Roles? Role { get; set; }
  public bool IsActive { get; set; } = true;
}
