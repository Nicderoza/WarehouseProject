using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Common.DTOs
{
  public class DTOCartItem
  {
    public int CartItemID { get; set; }  // <-- ID
    public int ProductID { get; set; }   // <-- deve arrivare pieno al client
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
  }

}
