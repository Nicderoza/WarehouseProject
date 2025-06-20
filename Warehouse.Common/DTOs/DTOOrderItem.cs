// Warehouse.Common.DTOs/DTOOrderItem.cs
namespace Warehouse.Common.DTOs
{
  public class DTOOrderItem
  {
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal UnitPrice { get; set; } 
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string SupplierName { get; set; }

  }
}
