
using System.Data;
using System.Security;

namespace Warehouse.Data.Models
{
  public class RolePermissions
  {
    public int RoleID { get; set; }
    public int PermissionID { get; set; }

    public Roles Role { get; set; }
    public Permissions Permission { get; set; }

  }
}
