using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
    public class Categories
    {
        [Key]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
