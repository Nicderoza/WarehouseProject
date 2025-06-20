// Warehouse.Common.DTOs/DTORegisterUser.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs
{
  public class DTORegisterUser
  {
    [Required(ErrorMessage = "L'email è obbligatoria.")]
    [EmailAddress(ErrorMessage = "Formato email non valido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La password è obbligatoria.")]
    [MinLength(6, ErrorMessage = "La password deve essere di almeno 6 caratteri.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Il cognome è obbligatorio.")]
    public string LastName { get; set; }
    public string SupplierName { get; set; }

    public DateTime? BirthDate { get; set; }
  }
}
