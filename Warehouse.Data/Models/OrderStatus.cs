using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
  public class OrderStatus
  {
    [Key]
    [Column("OrderStatusID")] // Mantieni il nome della colonna come "OrderStatusID"
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderStatusID { get; set; } // Usa OrderStatusID per coerenza

    [Required]
    [MaxLength(50)]
    public string Description { get; set; }

    public virtual ICollection<Orders> Orders { get; set; }
  }
}

