# ¿Qué es AutoMapper en C#?

**AutoMapper** es una biblioteca popular en .NET que permite mapear automáticamente los datos entre dos objetos, generalmente entre entidades del dominio y DTOs (Data Transfer Objects), o entre diferentes modelos de datos.

## ¿Por qué se suele usar?

- **Automatización del mapeo:** Evita la necesidad de escribir código manual para copiar propiedades de un objeto a otro.
- **Reducción de errores:** Minimiza los errores humanos al mapear propiedades, especialmente cuando los objetos tienen muchas propiedades similares.
- **Mantenimiento sencillo:** Facilita el mantenimiento del código, ya que los mapeos se configuran en un solo lugar y se actualizan fácilmente.
- **Limpieza del código:** Elimina el código repetitivo y mejora la legibilidad.

## ¿Qué significa "mapear"?

Mapear significa transferir los valores de las propiedades de un objeto a otro objeto, que puede tener una estructura similar o diferente. Por ejemplo, pasar los datos de una entidad `Category` a un `CategoryDto`.

## Ejemplo básico de uso

```csharp
// Configuración de AutoMapper
var configuration = new MapperConfiguration(cfg => {
    cfg.CreateMap<Category, CategoryDto>();
});
var mapper = configuration.CreateMapper();

// Uso para mapear objetos
Category category = new Category { Name = "Libros", Description = "Categoría de libros" };
CategoryDto dto = mapper.Map<CategoryDto>(category);
```

## Ventajas de AutoMapper

- **Rapidez en el desarrollo:** Permite crear mapeos complejos de forma rápida.
- **Consistencia:** Garantiza que los mapeos sean consistentes en toda la aplicación.
- **Flexibilidad:** Permite personalizar los mapeos según las necesidades del proyecto.

## Resumen

AutoMapper es una herramienta esencial en proyectos .NET donde se requiere transformar datos entre diferentes modelos, haciendo el proceso más eficiente, seguro y fácil de mantener.
