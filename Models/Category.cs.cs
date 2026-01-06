using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models;

public class Category
{
    [Key] // Indicate that Id is the primary key
    public int Id { get; set; }
    [Required] // Indicate that Name is required
    public string Name { get; set; } = string.Empty;
    [Required] // Indicate that CreationDate is required
    public DateTime CreationDate { get; set; }
}
