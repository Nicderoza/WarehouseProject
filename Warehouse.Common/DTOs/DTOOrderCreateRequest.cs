using Warehouse.Common.DTOs;
namespace Warehouse.Common.DTOs
{
  public class DTOOrderCreateRequest
  {
    public int UserID { get; set; } 
    public List<DTOOrderItemRequest> Items { get; set; }
    public decimal TotalAmount { get; set; }
  }
}
