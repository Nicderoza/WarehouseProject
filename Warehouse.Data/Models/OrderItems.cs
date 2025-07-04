using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Warehouse.Data.Models
{
  public class OrderItems
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderItemID { get; set; }

    [ForeignKey("Order")]
    public int OrderID { get; set; }
    [ForeignKey("Product")]
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string SupplierName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Orders Order { get; set; } 
    public virtual Products Product { get; set; } 
  }
}
