using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "The Name field is required.")] // Added Required attribute
    [MaxLength(50, ErrorMessage = "The Name field must not exceed 50 characters.")] // Added MaxLength attribute
    [MinLength(3, ErrorMessage = "The Name field must be at least 3 characters long.")] // Added MinLength attribute
    public string Name { get; set; } = string.Empty;
}
