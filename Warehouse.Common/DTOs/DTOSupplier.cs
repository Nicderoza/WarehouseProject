namespace Warehouse.Common.DTOs
{
    public class DTOSupplier
    {
        public int SupplierID { get; set; }
        public int CityID { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
