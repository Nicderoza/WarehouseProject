using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models
{
    public class Cities
    {
        [Key]  // Indica che CityID è la chiave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremento
        public int CityID { get; set; }

        public string Name { get; set; }

        public ICollection<Suppliers> Suppliers { get; set; }
    }
}
