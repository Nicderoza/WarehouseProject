namespace Warehouse.Data.Models
{
    public class RolePermissions
    {
        public required string Name {get; set;}
        public ICollection<Roles> Roles { get; set; }
    }
}
