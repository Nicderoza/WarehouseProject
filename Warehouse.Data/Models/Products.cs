using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Models;

public class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int? CategoryID { get; set; }
    public int? SupplierID { get; set; }

    public Categories Category { get; set; }
    public Suppliers Supplier { get; set; }
    public ICollection<Orders> Orders { get; set; }
    public virtual ICollection<OrderItems> OrderItems { get; set; }
}

