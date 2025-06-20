// Warehouse.Common.DTOs/DTOCreateUser.cs
using System;
using System.ComponentModel.DataAnnotations;

public class DTOCreateUser
{
  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Required]
  [MinLength(6)]
  public string Password { get; set; }

  [Required]
  public string Name { get; set; }    // Sarà popolato da FirstName del gestore
  [Required]
  public string Surname { get; set; } // Sarà popolato da LastName del gestore

  public DateTime? BirthDate { get; set; }

  [Required]
  public int RoleID { get; set; }
}
