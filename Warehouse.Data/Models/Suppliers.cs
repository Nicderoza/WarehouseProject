using System;
using System.Collections.Generic;

namespace Warehouse.Data.Models
{
  public class Suppliers
  {
    public int SupplierID { get; set; }

    public string CompanyName { get; set; } = string.Empty; 
    public int CityID { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public Cities? City { get; set; } 
    public ICollection<Products> Products { get; set; } = new List<Products>();
    public ICollection<UsersSuppliers> UsersSuppliers { get; set; } = new List<UsersSuppliers>();
  }
}
