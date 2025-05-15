using System.Collections.Generic;

namespace Warehouse.Data.Models
{
  public class Roles
  {
    public int RoleID { get; set; }
    public string RoleName { get; set; }

    // Relazione uno a molti con Users
    public ICollection<Users> Users { get; set; } // Correzione: Usa Users invece di ApplicationUser
  }
}
