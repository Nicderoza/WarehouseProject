using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
       
        public decimal Price { get; set; }

        public decimal TotalAmount => Quantity * Price; 
        public DateTime OrderDate { get; set; }
        public Users User { get; set; }
        public Products Product { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }


    }
}