namespace Warehouse.Common.DTOs
{
  public class DTOOrderItemRequest
  {
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
  }
}
