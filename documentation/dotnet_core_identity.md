# .NET Core Identity

## ¿Qué es .NET Core Identity?

.NET Core Identity es un sistema de autenticación y administración de usuarios que forma parte de ASP.NET Core. Permite gestionar usuarios, roles, contraseñas, claims y tokens de seguridad de manera integrada y segura.

## Características principales

- Registro y autenticación de usuarios
- Gestión de roles y permisos
- Recuperación y cambio de contraseñas
- Integración con autenticación externa (Google, Facebook, etc.)
- Soporte para claims y tokens JWT

## Instalación

Para agregar Identity a tu proyecto, ejecuta:

```
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

## Configuración básica

1. Agrega el contexto de Identity en tu `DbContext`:

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    // ... otras entidades
}
```

2. Configura Identity en `Program.cs`:

```csharp
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Identity configuration
```

## Uso común

- Registro de usuario
- Inicio de sesión
- Asignación de roles
- Protección de endpoints con `[Authorize]`

## Recursos útiles

- [Documentación oficial](https://learn.microsoft.com/es-es/aspnet/core/security/authentication/identity)
- [Tutorial de autenticación con Identity](https://learn.microsoft.com/es-es/aspnet/core/security/authentication/identity)

## Ejemplo de protección de controlador

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : Controller {
    // ...acciones protegidas
}
```

## Notas

- Identity puede personalizarse para usar entidades propias de usuario y rol.
- Se recomienda usar migraciones para crear las tablas necesarias en la base de datos.

---
