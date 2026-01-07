
// Esta clase se llama CategoryProfile y hereda de Profile, que es una clase especial de AutoMapper.
//
// ¿Por qué hereda de Profile?
// Porque Profile es como una "plantilla" donde le decimos a AutoMapper cómo debe copiar los datos entre diferentes objetos.
//
// Aquí le enseñamos al "robot" (AutoMapper) cómo pasar los datos de una caja (Category) a otra caja (CategoryDto),
// y también al revés, gracias a ReverseMap().
//
// ReverseMap() significa que el robot puede copiar los datos en ambos sentidos: de Category a CategoryDto y de CategoryDto a Category.
//
// Así, cuando queremos enviar información al cliente, el robot toma los datos de Category y los pone en CategoryDto automáticamente.
// Y si recibimos datos del cliente en un DTO, el robot puede volver a ponerlos en Category.
//
// Usamos Profile porque esta clase solo se dedica a definir estas reglas de mapeo. Si la clase tuviera otro propósito,
// heredaríamos de otra clase (por ejemplo, Controller para controladores, DbContext para contexto de base de datos, etc).

using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping;

public class CategoryProfile: Profile
{
   public CategoryProfile()
   {
      // Aquí configuramos el mapeo entre Category y CategoryDto, y también de CategoryDto a Category (por ReverseMap)
      CreateMap<Category, CategoryDto>().ReverseMap();
      // Lo mismo entre Category y CreateCategoryDto
      CreateMap<Category, CreateCategoryDto>().ReverseMap();
   }
}
