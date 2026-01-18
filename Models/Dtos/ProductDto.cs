namespace ApiEcommerce.Models.Dtos;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImgUrl { get; set; }
    public string? ImgUrlLocal { get; set; }
    public string SKU { get; set; } = string.Empty; // Stock Keeping Unit -> PROD-001-BLK-M
    public int StockQuantity { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;

     public int CategoryId { get; set; } // Foreign Key Property
     public string CategoryName { get; set; } = string.Empty; // To show Category Name directly in ProductDto
}
