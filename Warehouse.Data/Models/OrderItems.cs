using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
    public class OrderItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Potrebbe essere il prezzo al momento dell'ordine
                                           // Potresti avere altre proprietà specifiche per l'OrderItem
        public virtual Orders Order { get; set; } // Proprietà di navigazione all'ordine

        public virtual Products Product { get; set; } // Proprietà di navigazione al prodotto

    }
}