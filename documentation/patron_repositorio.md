# Documentación: Patrón Repositorio en C#

## ¿Qué es el patrón repositorio?

El **patrón repositorio** es un patrón de diseño que actúa como una capa intermedia entre la lógica de negocio y la fuente de datos (por ejemplo, una base de datos). Su objetivo es abstraer el acceso a los datos, centralizando la lógica de persistencia y permitiendo que el resto de la aplicación interactúe con los datos a través de una interfaz común.

## ¿Para qué se recomienda usarlo?

- **Separación de responsabilidades:** Permite separar la lógica de acceso a datos de la lógica de negocio.
- **Facilita pruebas unitarias:** Al abstraer el acceso a datos, es más sencillo crear implementaciones simuladas (mocks) para pruebas.
- **Centralización:** Toda la lógica de acceso a datos se encuentra en un solo lugar, facilitando el mantenimiento y la evolución del código.
- **Flexibilidad:** Permite cambiar la fuente de datos (por ejemplo, de una base de datos SQL a una NoSQL) sin afectar la lógica de negocio.

## Ejemplo básico en C#

### 1. Definir la interfaz del repositorio

```csharp
public interface IProductoRepository
{
    Producto GetById(int id);
    IEnumerable<Producto> GetAll();
    void Add(Producto producto);
    void Update(Producto producto);
    void Delete(int id);
}
```

### 2. Implementar el repositorio

```csharp
public class ProductoRepository : IProductoRepository
{
    private readonly ApplicationDbContext _context;

    public ProductoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Producto GetById(int id)
    {
        return _context.Productos.Find(id);
    }

    public IEnumerable<Producto> GetAll()
    {
        return _context.Productos.ToList();
    }

    public void Add(Producto producto)
    {
        _context.Productos.Add(producto);
        _context.SaveChanges();
    }

    public void Update(Producto producto)
    {
        _context.Productos.Update(producto);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var producto = _context.Productos.Find(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            _context.SaveChanges();
        }
    }
}
```

### 3. Uso en la lógica de negocio

```csharp
public class ProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public void CrearProducto(Producto producto)
    {
        _productoRepository.Add(producto);
    }
}
```

## Resumen

El patrón repositorio ayuda a mantener el código limpio, desacoplado y fácil de probar. Es ampliamente recomendado en aplicaciones que manejan acceso a datos, especialmente en proyectos de mediana y gran escala.

---

**Referencia:** [Documentación oficial de Microsoft sobre el patrón repositorio](https://learn.microsoft.com/es-es/aspnet/core/fundamentals/repository-pattern)
