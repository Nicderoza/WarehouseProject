using System;

namespace Warehouse.Common.DTOs
{
  public class DTOUser
  {
    public int UserID { get; set; }
    public int RoleID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; } // Resa nullable nel DTO
  }
}
