using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Models;

[Table("OrderStatus")]
public class OrderStatus
{
  [Key]
  [Column("OrderStatusID")] 
  public int OrderStatusID { get; set; } 

  [Required]
  [MaxLength(50)]
  [Column("StatusName")] 
  public string StatusName { get; set; }

  public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
