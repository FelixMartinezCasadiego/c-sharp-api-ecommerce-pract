using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping;

public class ProductProfile: Profile
{
    public ProductProfile()
    {
        // Este mapeo le dice a AutoMapper cómo convertir un objeto Product en un ProductDto.
        // Cuando convierte, toma el nombre de la categoría del producto (Product.Category.Name)
        // y lo pone en el campo CategoryName del ProductDto.
        // También permite hacer la conversión al revés (de ProductDto a Product).
        CreateMap<Product, ProductDto>().ForMember(dest => dest.CategoryName, 
        opt => opt.MapFrom(src => src.Category.Name)).ReverseMap(); // Map Category Name from Product to ProductDto
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();
    }
}
