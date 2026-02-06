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
    [Column(TypeName = "decimal(18,2)")] // Specify precision and scale for decimal
    public decimal Price { get; set; }
    public string? ImgUrl { get; set; }
    public string? ImgUrlLocal { get; set; }
    [Required]
    public string SKU { get; set; } = string.Empty; // Stock Keeping Unit -> PROD-001-BLK-M
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    [NotMapped]
    public int Stock
    {
        get => StockQuantity;
        set => StockQuantity = value;
    }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;

     // * Foreign Key with Category
     public int CategoryId { get; set; } // Foreign Key Property
     [ForeignKey("CategoryId")] // Reference to Category's Primary Key
     public required Category Category { get; set; } // Navigation Property
}
