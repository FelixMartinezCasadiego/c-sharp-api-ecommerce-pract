namespace ApiEcommerce.Models.Dtos;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty; // Stock Keeping Unit -> PROD-001-BLK-M
    public int StockQuantity { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;

     public int CategoryId { get; set; } // Foreign Key Property
}
