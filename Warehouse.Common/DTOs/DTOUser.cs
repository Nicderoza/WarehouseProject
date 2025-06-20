// Warehouse.Common.DTOs/DTOUser.cs
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
    public bool IsActive { get; set; } = true;

    public string PasswordHash { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; } // <--- Corretto: È DateTime, coerente con Users e UserService
  }
}
/*
 *{
    "Email":"peppinobianco@libero.it",
    "Password":"abc123!!",
    "token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwZXBwaW5vYmlhbmNvQGxpYmVyby5pdCIsImp0aSI6ImQ3ZmRhMWRmLWRlZDEtNDA2Yy1hNTQ1LWQ3ZGFlNGI1MDE4YiIsIlVzZXJJRCI6IjIwMDIiLCJleHAiOjE3NDc3MzYwMjAsImlzcyI6IldhcmVob3VzZSIsImF1ZCI6ImlDbGllbnREZWxsYVR1YUFwcGxpY2F6aW9uZSJ9.gLE5jMLCQMaJ-jf9SVJlQ-PFY92EWRmkwSLA9rTdLMw"

}
{
    "Email":"peppinobianco@libero.it",
    "CurrentPassword":"abc123!!",
    "NewPassword":"def456!!"
}
*/
