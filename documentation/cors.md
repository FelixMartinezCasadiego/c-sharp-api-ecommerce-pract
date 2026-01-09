# CORS (Cross-Origin Resource Sharing)

## ¿Qué significa CORS?

CORS (Cross-Origin Resource Sharing) es un mecanismo de seguridad implementado en los navegadores web que permite controlar cómo los recursos de una página web pueden ser solicitados desde un dominio diferente al que sirve la página.

## ¿Por qué suele usarse CORS?

Por defecto, los navegadores restringen las solicitudes HTTP realizadas desde scripts que se ejecutan en una página web a recursos que no pertenecen al mismo dominio, protocolo y puerto. Esto se conoce como la política del mismo origen (Same-Origin Policy). Sin embargo, en aplicaciones modernas, es común que el frontend y el backend estén alojados en diferentes dominios o puertos. CORS permite que el servidor autorice explícitamente qué orígenes externos pueden acceder a sus recursos.

## Objetivo de CORS

El objetivo principal de CORS es mejorar la seguridad de las aplicaciones web, evitando que sitios maliciosos puedan acceder a datos sensibles de otros sitios sin permiso. Al mismo tiempo, permite la interoperabilidad entre servicios web distribuidos, facilitando el desarrollo de aplicaciones con arquitecturas desacopladas (por ejemplo, un frontend en React y un backend en .NET).

## Ejemplo de uso

## Implementación de CORS en este proyecto

En el archivo Program.cs de este proyecto, se configuró CORS de la siguiente manera:

```csharp
builder.Services.AddCors(options => // Configuración de CORS
{
    options.AddPolicy("AllowSpecificOrigin", builder => // Política CORS
    {
        builder.WithOrigins("*") // Permite cualquier origen
               .AllowAnyMethod() // Permite cualquier método
               .AllowAnyHeader(); // Permite cualquier encabezado
    });
});

// ...

app.UseCors("AllowSpecificOrigin"); // Aplicación de la política CORS
```

### Explicación

1. Se agrega el servicio CORS en el contenedor de servicios, definiendo una política llamada "AllowSpecificOrigin" que permite solicitudes desde cualquier origen (`*`), cualquier método y cualquier encabezado.
2. Se aplica la política CORS antes de la autorización y el mapeo de controladores, permitiendo que las solicitudes externas accedan a la API según la configuración definida.

Esta configuración es útil en entornos de desarrollo o cuando se desea exponer la API a múltiples orígenes. En producción, se recomienda restringir los orígenes permitidos para mejorar la seguridad.

## Referencias

## Uso de CORS en los Controllers

Además de la configuración global, es posible habilitar CORS de manera específica en controladores o acciones usando el atributo `[EnableCors]`. Esto permite aplicar políticas de CORS solo a ciertos endpoints.

### Ejemplo en un Controller

```csharp
using Microsoft.AspNetCore.Cors;

[EnableCors(PolicyNames.AllowSpecificOrigin)] // Habilita CORS solo para este controlador o acción
public class CategoriesController : ControllerBase
{
    // ...acciones del controlador...
}
```

En este ejemplo, el atributo `[EnableCors(PolicyNames.AllowSpecificOrigin)]` indica que el controlador `CategoriesController` permite solicitudes CORS según la política definida, independientemente de la configuración global. Esto es útil cuando se requiere un control más granular sobre qué endpoints exponen CORS.

Puedes aplicar este atributo a nivel de clase (controlador) o a nivel de método (acción) según la necesidad.
