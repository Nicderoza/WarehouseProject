using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Common.DTOs.DTOAdmin
{
    public class DTOChangeRole
    {
    public int UserID { get; set; } 
    public string NewRole { get; set; } = null!;
  }
}
