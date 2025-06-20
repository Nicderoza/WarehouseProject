using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Warehouse.Data.Models
{
  public class Permissions
  {
    [Key]
    public int PermissionID { get; set; }

    [Required, MaxLength(100)]
    public string PermissionName { get; set; }

    public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    public virtual ICollection<Roles> Roles { get; set; } = new List<Roles>();

  }
}
