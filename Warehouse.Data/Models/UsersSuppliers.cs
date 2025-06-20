namespace Warehouse.Data.Models
{
  public class UsersSuppliers
  {
    public int UserID { get; set; }
    public int SupplierID { get; set; }

    public Users User { get; set; }
    public Suppliers Supplier { get; set; }
  }
}
