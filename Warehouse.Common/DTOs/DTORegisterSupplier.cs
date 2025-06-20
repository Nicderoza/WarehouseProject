// Warehouse.Common.DTOs/DTORegisterSupplier.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs
{
  public class DTORegisterSupplier
  {
    [Required(ErrorMessage = "L'email è obbligatoria.")]
    [EmailAddress(ErrorMessage = "Formato email non valido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La password è obbligatoria.")]
    [MinLength(6, ErrorMessage = "La password deve essere di almeno 6 caratteri.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Il nome del gestore è obbligatorio.")]
    public string FirstName { get; set; } // Nome del gestore

    [Required(ErrorMessage = "Il cognome del gestore è obbligatorio.")]
    public string LastName { get; set; } // Cognome del gestore

    public DateTime? BirthDate { get; set; } // Data di nascita del gestore (opzionale)

    [Required(ErrorMessage = "Il nome dell'azienda è obbligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Il nome dell'azienda deve avere tra 2 e 100 caratteri.")]
    public string CompanyName { get; set; } // Il nome dell'azienda fornitore

    [Range(1, int.MaxValue, ErrorMessage = "L'ID della città deve essere un numero positivo.")]
    public int? CityID { get; set; }

  }
}
