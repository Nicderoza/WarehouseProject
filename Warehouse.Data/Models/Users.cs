using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
  public class Users
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    public int RoleID { get; set; }

    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; } 

    public ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public Roles? Role { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<UsersSuppliers> UsersSuppliers { get; set; } = new List<UsersSuppliers>();
    public virtual Cart Cart { get; set; } // Proprietà di navigazione al carrello

  }
}
