# Autenticación y protección de rutas con JWT en Program.cs

## Descripción general

En este proyecto, la autenticación y protección de rutas se realiza utilizando JWT (JSON Web Tokens). La configuración principal se encuentra en el archivo `Program.cs`, donde se define cómo la API valida y acepta los tokens JWT para proteger los endpoints.

## Flujo principal de configuración

1. **Obtención de la clave secreta**

   - Se obtiene la clave secreta desde la configuración (`appsettings.json` o variables de entorno) para firmar y validar los tokens JWT.
   - Si la clave no está presente, se lanza una excepción para evitar que la aplicación arranque sin seguridad.

2. **Configuración de la autenticación**

   - Se establece el esquema de autenticación predeterminado como `JwtBearer`.
   - Se configura el middleware para aceptar y validar tokens JWT en las solicitudes.

3. **Parámetros de validación del token**
   - Se valida la clave de firma del emisor (`IssuerSigningKey`).
   - Se puede habilitar o deshabilitar la validación del emisor y del público (audience).
   - Se define la clave de firma simétrica usando la clave secreta.
   - Se configuran los parámetros de autoridad y audiencia desde la configuración (útil para integración con servicios como Auth0).

## Ejemplo de código

```csharp
var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Secret key not found in configuration.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = true,
    };
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});
```

## Explicación de cada opción

### options en AddAuthentication

- **DefaultAuthenticateScheme**: Especifica el esquema de autenticación predeterminado que se usará para autenticar a los usuarios. En este caso, se utiliza `JwtBearerDefaults.AuthenticationScheme`, lo que indica que se usará JWT Bearer.
- **DefaultChallengeScheme**: Especifica el esquema predeterminado que se usará cuando se requiera un desafío de autenticación (por ejemplo, cuando un usuario no autenticado intenta acceder a un recurso protegido).

### options en AddJwtBearer

- **RequireHttpsMetadata**: Si es `true`, solo se aceptarán tokens a través de HTTPS. En desarrollo puede ponerse en `false` para permitir HTTP, pero en producción debe ser `true` para mayor seguridad.
- **SaveToken**: Si es `true`, el token recibido se almacena en el contexto de autenticación y puede ser accedido posteriormente.
- **TokenValidationParameters**: Conjunto de parámetros que definen cómo se valida el token JWT:
  - **ValidateIssuerSigningKey**: Indica si se debe validar la clave de firma del emisor del token.
  - **IssuerSigningKey**: Clave simétrica utilizada para validar la firma del token.
  - **ValidateIssuer**: Indica si se debe validar el emisor del token (`iss`).
  - **ValidateAudience**: Indica si se debe validar la audiencia del token (`aud`).
- **Authority**: Dirección del proveedor de identidad (por ejemplo, Auth0) que emite los tokens. Se utiliza para validar el emisor y obtener metadatos de configuración.
- **Audience**: Especifica la audiencia esperada del token. Solo los tokens emitidos para esta audiencia serán aceptados.

## ¿Cómo protege las rutas?

- Al configurar la autenticación JWT, los endpoints de la API pueden ser protegidos usando el atributo `[Authorize]` en los controladores o acciones.
- Solo los usuarios que presenten un token JWT válido podrán acceder a las rutas protegidas.

## Protección por roles con [Authorize(Roles = "Admin")]

Además de proteger rutas con autenticación, es posible restringir el acceso a ciertos endpoints solo a usuarios con roles específicos usando `[Authorize(Roles = "Admin")]`.

### Ejemplo de uso

```csharp
[Authorize(Roles = "Admin")]
public IActionResult DeleteUser(int id)
{
    // Solo los usuarios con el rol "Admin" pueden acceder a esta acción
}
```

### Explicación

- El atributo `[Authorize(Roles = "Admin")]` indica que solo los usuarios autenticados cuyo token JWT contenga el rol "Admin" podrán acceder a ese controlador o acción.
- Puedes especificar varios roles separados por coma: `[Authorize(Roles = "Admin,Manager")]`.
- Es útil para implementar control de acceso basado en roles (RBAC) y proteger operaciones sensibles o administrativas.

## Permitir acceso anónimo con [AllowAnonymous]

En algunos casos, es necesario que ciertos endpoints sean accesibles sin autenticación, incluso si el controlador está protegido con `[Authorize]`. Para esto se utiliza el atributo `[AllowAnonymous]`.

### Ejemplo de uso

```csharp
[Authorize] // Protege todo el controlador
public class CategoriesController : ControllerBase
{
    [AllowAnonymous] // Permite acceso sin autenticación a esta acción
    [HttpGet]
    public IActionResult GetCategories() { /* ... */ }
    // ...otras acciones protegidas...
}
```

### Explicación

- `[AllowAnonymous]` anula la protección de `[Authorize]` solo para la acción o controlador donde se aplica, permitiendo el acceso a usuarios no autenticados.
- Es útil para exponer endpoints públicos (por ejemplo, listados públicos, login, registro) mientras se mantiene la protección en el resto de la API.

## Ventajas de usar JWT

- Permite la autenticación sin estado (stateless), ideal para APIs REST.
- Los tokens pueden ser generados y validados fácilmente.
- Facilita la integración con proveedores externos como Auth0.

## Referencias

- [JWT.io: Introducción a JSON Web Tokens](https://jwt.io/introduction)
