// Warehouse.Common.DTOs/DTOCartUpdateItemQuantityRequest.cs
using System;
using System.ComponentModel.DataAnnotations; // Aggiungi questo using

namespace Warehouse.Common.DTOs
{
  public class DTOCartUpdateItemQuantityRequest
  {
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CartItemId must be greater than 0.")]
    public int CartItemID { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0.")]
    public int Quantity { get; set; } // La quantità può essere 0 per indicare rimozione
  }
}
