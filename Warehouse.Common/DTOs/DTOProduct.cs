namespace Warehouse.Common.DTOs
{
    public class DTOProduct
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public decimal Price { get; set; }
    public ICollection<DTOOrderItem> OrderItems { get; set; }

  }
}
