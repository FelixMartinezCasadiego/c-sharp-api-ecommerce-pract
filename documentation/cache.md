# Documentación sobre Cache en APIs

## ¿Qué es Cache?

El cache es un mecanismo de almacenamiento temporal que permite guardar datos o respuestas para que futuras solicitudes puedan ser atendidas más rápidamente, evitando cálculos o consultas repetitivas.

## ¿Para qué se usa Cache en una API?

En una API, el cache se utiliza para:

- **Mejorar el rendimiento:** Almacenar respuestas frecuentes y reducir el tiempo de procesamiento.
- **Reducir la carga en el servidor y la base de datos:** Disminuye el número de consultas directas a la base de datos o cálculos intensivos.
- **Optimizar la experiencia del usuario:** Las respuestas son más rápidas y consistentes.
- **Minimizar el uso de recursos:** Menos uso de CPU, memoria y ancho de banda.

## Tipos de Cache en APIs

- **Cache en memoria:** Utiliza la RAM del servidor para almacenar datos temporales (ejemplo: Redis, MemoryCache).
- **Cache distribuido:** Permite compartir datos cacheados entre varios servidores.
- **Cache en el cliente:** El navegador o consumidor de la API almacena respuestas para evitar solicitudes repetidas.
- **Cache de respuesta HTTP:** Utiliza cabeceras como `Cache-Control` y `ETag` para controlar el almacenamiento y la validez de las respuestas.

## Consideraciones

---

**Referencias:**

- [Microsoft Docs: Response caching in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-7.0)
- [Middleware CACHE](https://learn.microsoft.com/es-mx/aspnet/core/performance/caching/middleware?view=aspnetcore-9.0)
- [ResponseCacheAttribute en ASP.NET Core](https://learn.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.mvc.responsecacheattribute?view=aspnetcore-10.0)

## Configuración de Cache en Program.cs

En este proyecto, la cache de respuestas HTTP se configura en el archivo Program.cs utilizando el método `AddResponseCaching`. Esto permite almacenar en memoria las respuestas de la API para mejorar el rendimiento.

### Ejemplo de configuración:

```csharp
builder.Services.AddResponseCaching(options =>
{
	options.MaximumBodySize = 1024;
	options.UseCaseSensitivePaths = true;
});
```

**Explicación:**

- `MaximumBodySize`: Define el tamaño máximo (en bytes) de la respuesta que se puede almacenar en cache.
- `UseCaseSensitivePaths`: Indica si las rutas deben ser sensibles a mayúsculas/minúsculas al almacenar en cache.

### Uso en los controladores

Para que la cache funcione, debes agregar el atributo `[ResponseCache]` en los métodos de los controladores que deseas cachear. Ejemplo:

```csharp
[HttpGet]
[ResponseCache(Duration = 60)] // Cachea la respuesta por 60 segundos
public IActionResult Get()
{
	// ...código...
}
```

### Activación del middleware

No olvides agregar la línea `app.UseResponseCaching();` en la configuración del pipeline de la aplicación (en Program.cs) para activar el middleware de cache.

```csharp
app.UseResponseCaching();
```

---

Consulta la documentación oficial para más detalles y buenas prácticas.
