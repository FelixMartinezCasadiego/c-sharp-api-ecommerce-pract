# ¿Qué es un Controller en .NET?

## Tipos de Controllers en .NET

En .NET, existen principalmente dos tipos de controllers cuando creas una aplicación:

- **Controller de API:**

  - Se usa en aplicaciones que exponen servicios web (APIs REST).
  - Hereda de `ControllerBase`.
  - Devuelve datos en formatos como JSON o XML, ideales para comunicación con clientes como aplicaciones móviles, web o de escritorio.
  - Se identifica por el atributo `[ApiController]`.

- **Controller de MVC:**
  - Se usa en aplicaciones web tradicionales que devuelven vistas HTML al navegador.
  - Hereda de `Controller`.
  - Devuelve vistas (archivos .cshtml) y puede también manejar datos.
  - Se utiliza para construir páginas web completas con interfaz de usuario.

Ambos tipos organizan la lógica de la aplicación, pero el de API está enfocado en servicios y el de MVC en la presentación de páginas web.

Un **Controller** (controlador) en .NET es una clase especial que se utiliza en aplicaciones web (como ASP.NET Core) para manejar las solicitudes (requests) que llegan desde los usuarios o clientes. El controlador recibe la petición, procesa la lógica necesaria y devuelve una respuesta.

## ¿Para qué se usa?

- **Gestionar solicitudes HTTP:** Los controladores reciben y responden a solicitudes como GET, POST, PUT, DELETE, etc.
- **Organizar la lógica de la aplicación:** Permiten separar la lógica de negocio y la presentación, facilitando el mantenimiento del código.
- **Comunicación con modelos y servicios:** Los controladores suelen interactuar con modelos de datos y servicios para obtener, modificar o eliminar información.
- **Enviar respuestas:** Devuelven datos al cliente, ya sea en formato HTML, JSON, XML, etc.

## Ejemplo básico

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    public IActionResult GetCategories()
    {
        // Lógica para obtener categorías
        return Ok(new List<string> { "Libros", "Ropa" });
    }
}
```

## Recomendaciones para usar Controllers

- **Mantén los controladores simples:** Evita poner demasiada lógica en el controlador. Usa servicios para la lógica de negocio.
- **Usa atributos para rutas y métodos:** Como `[Route]`, `[HttpGet]`, `[HttpPost]`, etc., para definir cómo se accede a cada acción.
- **Valida los datos de entrada:** Antes de procesar la información, valida los datos recibidos del cliente.
- **Devuelve respuestas claras:** Usa métodos como `Ok()`, `BadRequest()`, `NotFound()` para enviar respuestas adecuadas.
- **Organiza por funcionalidad:** Crea un controlador para cada área principal de tu aplicación (por ejemplo, CategoryController, ProductController).

## Resumen

Un controller en .NET es el punto de entrada para las solicitudes web y es fundamental para estructurar y organizar la lógica de tu aplicación. Usar buenas prácticas en los controladores mejora la calidad, seguridad y mantenibilidad del proyecto.
