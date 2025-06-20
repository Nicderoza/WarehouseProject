using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
namespace Warehouse.Data.Models
{
  public class CartItems
  {
    public int ID { get; set; }
    public int CartID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    [Precision(18, 4)]
    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Products Product { get; set; }
    public Cart Cart { get; set; }
  }
}
