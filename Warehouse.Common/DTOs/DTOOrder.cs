namespace Warehouse.Common.DTOs
{
    public class DTOOrder
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; } // Nuova proprietà
        public DateTime OrderDate { get; set; }

    }
}