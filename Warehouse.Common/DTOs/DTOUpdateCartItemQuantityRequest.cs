using System.ComponentModel.DataAnnotations;

namespace Warehouse.Common.DTOs
  
{
    public class DTOUpdateCartItemQuantityRequest
    {
    [Required(ErrorMessage = "Cart Item ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Cart Item ID must be a positive number.")]
    public int CartItemID { get; set; } // L'ID dell'elemento specifico nel carrello

    [Required(ErrorMessage = "New quantity is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")] // 0 per rimuovere l'articolo
    public int Quantity { get; set; }
  }
}
