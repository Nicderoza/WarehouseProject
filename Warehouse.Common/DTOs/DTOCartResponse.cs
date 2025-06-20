namespace Warehouse.Common.DTOs
{
    public class DTOCartResponse
    {
    public int CartID { get; set; }
    public int UserID { get; set; }
    public int OrderID { get; set; }
    public decimal TotalAmount { get; set; }
    public string StatusName { get; set; }
    public List<DTOCartItem> Items { get; set; } = new List<DTOCartItem>(); 
  }
}
