namespace Warehouse.Common.DTOs
{
    public class DTOOwner
    {
        public int OwnerID { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}
