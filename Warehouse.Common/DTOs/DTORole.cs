namespace Warehouse.Common.DTOs
{
    class DTORole
    {
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public List<DTOPermission> Permissions { get; set; } = new();
  }
}
