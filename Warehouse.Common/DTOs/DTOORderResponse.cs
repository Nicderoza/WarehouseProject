using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Common.DTOs
{
  public class DTOOrderResponse
  {
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
    public List<DTOOrderItem> Items { get; set; }
  }

}
