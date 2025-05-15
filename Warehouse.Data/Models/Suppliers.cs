using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Models;

public class Suppliers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupplierID { get; set; }
    public int CityID { get; set; }

    public string Name { get; set; }

    public Cities City { get; set; }

    public ICollection<Products> Products { get; set; }
}
