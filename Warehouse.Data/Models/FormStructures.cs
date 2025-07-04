using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Models
{
  public class FormStructures
  {
    [Key]
    public int FormID { get; set; }
    public string ProductForm { get; set; }
    public string JsonContent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
