using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    [Required]
    public string SKU { get; set; } = string.Empty; // Stock Keeping Unit -> PROD-001-BLK-M
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;

     // * Foreign Key with Category
     public int CategoryId { get; set; } // Foreign Key Property
     [ForeignKey("Id")] // Reference to Category's Primary Key
     public required Category Category { get; set; } // Navigation Property
}
