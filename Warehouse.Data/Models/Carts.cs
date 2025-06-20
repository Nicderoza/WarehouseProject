using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Data.Models
{
  [Table("Carts")]
  public class Cart
  {
    [Key]
    [Column("ID")]
    public int CartID { get; set; }

    [Column("UserID")]
    public int UserID { get; set; }
    public int OrderID { get; set; }
    public decimal TotalAmount { get; set; }

    [ForeignKey("UserID")]
    public virtual Users User { get; set; }

    [Required]
    [Column("Status")]
    public CartStatus Status { get; set; }

    [Required]
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("UpdatedAt")]
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

  }
}
