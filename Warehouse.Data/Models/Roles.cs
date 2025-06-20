using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Warehouse.Data.Models
{
  public class Roles
  {
    [Key]
    public int RoleID { get; set; }

    [Required, MaxLength(50)]
    public string RoleName { get; set; }

    public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    public virtual ICollection<Permissions> Permissions { get; set; } = new List<Permissions>();
  }
}
