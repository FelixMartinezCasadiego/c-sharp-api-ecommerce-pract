# Versionamiento de APIs en .NET

## ¿Qué es el versionamiento de APIs?

El versionamiento de APIs es una práctica fundamental en el desarrollo de servicios web. Consiste en gestionar y mantener diferentes versiones de una API para garantizar la compatibilidad con clientes antiguos mientras se introducen nuevas funcionalidades o cambios que podrían romper la compatibilidad.

### ¿Por qué es importante?

- Permite evolucionar la API sin afectar a los consumidores existentes.
- Facilita la corrección de errores y la introducción de mejoras.
- Ayuda a mantener la estabilidad y previsibilidad del servicio.

## Opciones de versionamiento en general

1. **Versionamiento en la URL**: Incluir la versión en la ruta, por ejemplo: `/api/v1/productos`.
2. **Versionamiento en el encabezado (Header)**: Especificar la versión en un header personalizado, por ejemplo: `api-version: 1.0`.
3. **Versionamiento en el parámetro de consulta**: Usar un query string, por ejemplo: `/api/productos?api-version=1.0`.
4. **Versionamiento en el cuerpo de la petición**: Menos común, pero posible en algunos escenarios.

## Versionamiento de APIs en .NET

### Paquetes recomendados para versionamiento en .NET

Para implementar el versionamiento de APIs en .NET de forma moderna y flexible, se recomienda instalar los siguientes paquetes NuGet:

- **Asp.Versioning.Mvc**
- **Asp.Versioning.Mvc.ApiExplorer**

Estos paquetes permiten gestionar versiones de controladores y exponer la información de versionado en herramientas como Swagger/OpenAPI.

## Enfoques de versionamiento de APIs

- **En la ruta** (`/api/v1/productos`)

| En la URL         | Con parámetro en query | En el header       |
| ----------------- | ---------------------- | ------------------ |
| `/api/v1/recurso` | `?api-version=1.0`     | `api-version: 1.0` |

Estos son los enfoques más comunes para versionar una API, permitiendo flexibilidad y claridad para los consumidores del servicio.

### Opciones en .NET

1. **Versionamiento por URL**

   - Ejemplo: `/api/v1/productos`
   - Se define la versión como parte de la ruta en los controladores.

2. **Versionamiento por encabezado**

   - Se configura para que la versión se especifique en un header HTTP.
   - Ejemplo: `api-version: 1.0`

3. **Versionamiento por parámetro de consulta**
   - La versión se pasa como un parámetro en la URL.
   - Ejemplo: `/api/productos?api-version=1.0`

### Implementación básica en .NET

1. Instalar el paquete:
   ```bash
   dotnet add package Microsoft.AspNetCore.Mvc.Versioning
   ```
2. Configurar en `Program.cs` o `Startup.cs`:
   ```csharp
   builder.Services.AddApiVersioning(options =>
   {
       options.DefaultApiVersion = new ApiVersion(1, 0);
       options.AssumeDefaultVersionWhenUnspecified = true;
       options.ReportApiVersions = true;
       options.ApiVersionReader = new UrlSegmentApiVersionReader(); // O HeaderApiVersionReader, QueryStringApiVersionReader
   });
      // Configuración del explorador de versiones para Swagger/OpenAPI
      apiVersioingBuilder.AddApiExplorer(options => // Configuración del API explorer
      {
         options.GroupNameFormat = "'v'VVV"; // Formato de nombre de grupo: v1, v2, v3, etc.
         options.SubstituteApiVersionInUrl = true; // Sustituye la versión en la URL: api/v{version}/[controller]
      });
   ```
3. Decorar los controladores:

```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class CategoriesController : ControllerBase
{
   // Acción para la versión 1.0
   [HttpGet]
   [MapToApiVersion("1.0")]
   public IActionResult GetCategories() { /* ... */ }

   // Acción para la versión 2.0
   [HttpGet]
   [MapToApiVersion("2.0")]
   public IActionResult GetCategoriesOrderById() { /* ... */ }
   // ...otras acciones...
}
```

#### Explicación de los atributos de versionado en el controlador

- `[ApiVersion("1.0")]` y `[ApiVersion("2.0")]`: Indican que el controlador responde a las versiones 1.0 y 2.0 de la API.
- `[Route("api/v{version:apiVersion}/[controller]")]`: Define la ruta para incluir la versión en la URL.
- `[MapToApiVersion("1.0")]` y `[MapToApiVersion("2.0")]`: Permiten mapear acciones específicas a una versión determinada de la API, facilitando la evolución de los endpoints sin romper compatibilidad con versiones anteriores.

#### ¿Para qué se usa AddApiExplorer?

El método `AddApiExplorer` permite que las herramientas de documentación como Swagger/OpenAPI reconozcan y agrupen los endpoints según la versión de la API. Esto facilita la visualización y prueba de diferentes versiones desde la interfaz de Swagger, mostrando cada versión como un grupo separado y permitiendo probar rutas como `/api/v1/productos` o `/api/v2/productos` de forma independiente.

```csharp
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductosController : ControllerBase
{
    // ...
}
```
