using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs
{
  public class DTOCreateUser
  {
    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    [MinLength(8)] // Esempio di validazione
    public string Password { get; set; }

    // Altre proprietà necessarie per la creazione
    public int? RoleID { get; set; }
    public DateTime BirthDate { get; set; }
  }
}
