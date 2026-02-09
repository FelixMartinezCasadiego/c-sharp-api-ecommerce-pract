# Paginación en la API de Ecommerce

La paginación es una técnica utilizada para dividir grandes conjuntos de datos en partes más pequeñas (páginas), facilitando así la consulta y visualización eficiente de los registros.

## ¿Dónde se implementa la paginación?

La lógica de paginación se encuentra implementada en el método `GetProductsInPages` dentro del repositorio de productos (`ProductRepository`).

### Método relevante

```csharp
public ICollection<Product> GetProductsInPages(int pageNumber, int pageSize)
{
    return _db.Products.OrderBy(p => p.ProductId)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();
}
```

### Explicación de la lógica
- **OrderBy(p => p.ProductId):** Ordena los productos por su identificador para garantizar un orden consistente.
- **Skip((pageNumber - 1) * pageSize):** Omite los primeros N registros, donde N es el número de elementos que ya han sido mostrados en páginas anteriores. Por ejemplo, si estás en la página 3 y el tamaño de página es 10, se omiten los primeros 20 productos.
- **Take(pageSize):** Toma la cantidad de productos correspondiente al tamaño de página solicitado.
- **ToList():** Convierte el resultado en una lista de productos.

### Parámetros
- `pageNumber`: Número de la página que se desea obtener (comienza en 1).
- `pageSize`: Cantidad de productos por página.

### Ejemplo de uso
Si se solicita la página 2 con un tamaño de página de 5:
- Se omiten los primeros 5 productos (`Skip(5)`).
- Se obtienen los siguientes 5 productos (`Take(5)`).

### Beneficios de la paginación
- Mejora el rendimiento de la API.
- Reduce la cantidad de datos transferidos en cada solicitud.
- Facilita la navegación y visualización de grandes volúmenes de información.

---

**Referencia:**
- Archivo: `Repository/ProductRepository.cs`
- Método: `GetProductsInPages(int pageNumber, int pageSize)`
