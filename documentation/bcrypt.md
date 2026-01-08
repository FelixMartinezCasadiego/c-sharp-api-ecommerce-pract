# BCrypt.Net-Next

## ¿Qué es?

BCrypt.Net-Next es una biblioteca para .NET que permite realizar el hash (encriptación) y la verificación de contraseñas de manera segura utilizando el algoritmo BCrypt.

## ¿Para qué se usa?

Se utiliza principalmente para almacenar contraseñas de usuarios de forma segura en bases de datos, evitando guardar contraseñas en texto plano. BCrypt es resistente a ataques de fuerza bruta y ataques de diccionario gracias a su función de coste configurable.

## Instalación

```bash
dotnet add package BCrypt.Net-Next
```

## Principales métodos (props)

- `BCrypt.HashPassword(string password)`: Genera un hash seguro a partir de una contraseña en texto plano.
- `BCrypt.Verify(string password, string hash)`: Verifica si una contraseña en texto plano coincide con un hash almacenado.
- `BCrypt.GenerateSalt(int workFactor = 10)`: Genera un salt personalizado, donde `workFactor` define la complejidad (por defecto 10).

## Ejemplo de uso

```csharp
using BCrypt.Net;

string password = "miContraseñaSegura";
string hash = BCrypt.HashPassword(password);

bool isValid = BCrypt.Verify(password, hash); // true si la contraseña es correcta
```

## Más información

- [Repositorio oficial en GitHub](https://github.com/BcryptNet/bcrypt.net)
- [Documentación NuGet](https://www.nuget.org/packages/BCrypt.Net-Next/)
