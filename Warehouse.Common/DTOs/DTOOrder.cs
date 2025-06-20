// Warehouse.Common.DTOs/DTOOrder.cs
using System;
using System.Collections.Generic;

namespace Warehouse.Common.DTOs
{
  public class DTOOrder
  {
    public int OrderID { get; set; }
    public int UserID { get; set; }

    public int OrderStatusID { get; set; } 
    public string StatusName { get; set; } 

    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public List<DTOCartItem> Items { get; set; }
    public List<DTOOrderItem> OrderItems { get; set; } = new List<DTOOrderItem>();

  }
}
