# JWT (JSON Web Token) en C#

## ¿Qué es JWT?

JWT (JSON Web Token) es un estándar abierto (RFC 7519) que define un método compacto y seguro para transmitir información entre partes como un objeto JSON. Esta información puede ser verificada y confiable porque está firmada digitalmente.

## Características clave de JWT

- **Autocontenible:** Un JWT es autocontenible, lo que significa que incluye toda la información necesaria sobre el usuario o la sesión dentro del propio token. El servidor no necesita consultar una base de datos para validar la información, ya que todo lo relevante está en el token.
- **Firmado:** Los JWT pueden ser firmados digitalmente usando algoritmos como HMAC o RSA, lo que garantiza la integridad y autenticidad del token. Solo el emisor legítimo puede generar una firma válida.
- **Cifrado:** Además de ser firmados, los JWT pueden ser cifrados para proteger la confidencialidad de la información que contienen. Esto asegura que solo las partes autorizadas puedan leer el contenido del token.

## ¿Para qué se usa JWT?

JWT se utiliza principalmente para:

- **Autenticación:** Permite identificar a un usuario de manera segura en aplicaciones web y APIs.
- **Autorización:** Controla el acceso a recursos protegidos después de la autenticación.
- **Intercambio de información:** Transmite datos entre partes de forma segura y confiable.

## ¿Cómo se usa JWT en C#?

En aplicaciones .NET, JWT se usa comúnmente para implementar autenticación basada en tokens. El flujo típico es:

1. El usuario se autentica (por ejemplo, con usuario y contraseña).
2. El servidor genera un JWT y lo envía al cliente.
3. El cliente almacena el token (usualmente en localStorage o sessionStorage).
4. El cliente envía el token en el encabezado Authorization en cada solicitud.
5. El servidor valida el token en cada solicitud protegida.

### Ejemplo básico de uso en C#

#### 1. Instalación del paquete

```bash
// Usar NuGet para instalar el paquete
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
```

#### 2. Configuración en `Program.cs` o `Startup.cs`

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "tu_issuer",
        ValidAudience = "tu_audience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tu_clave_secreta"))
    };
});
```

#### 3. Generación de un JWT

```csharp
var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, userId),
    new Claim(JwtRegisteredClaimNames.Email, userEmail)
};
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tu_clave_secreta"));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
var token = new JwtSecurityToken(
    issuer: "tu_issuer",
    audience: "tu_audience",
    claims: claims,
    expires: DateTime.Now.AddHours(1),
    signingCredentials: creds
);
string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
```

#### 4. Validación automática

El middleware de autenticación de ASP.NET Core valida automáticamente el JWT en las solicitudes protegidas.

## Propiedades principales de un JWT

## Estructura de un JWT

Un JWT está compuesto por tres partes, separadas por puntos (`.`):

```
header.payload.signature
```

Cada parte tiene un propósito específico:

| Parte     | Descripción                                                           | Ejemplo (Base64Url)                                                        |
| --------- | --------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| Header    | Indica el tipo de token y el algoritmo de firma usado.                | eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9                                       |
| Payload   | Contiene los claims o datos del usuario y otra información relevante. | eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ |
| Signature | Firma digital generada usando el header, payload y una clave secreta. | SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c                                |

### Detalle de cada parte

| Parte     | Contenido principal | Ejemplo JSON / Descripción                                                  |
| --------- | ------------------- | --------------------------------------------------------------------------- |
| Header    | Algoritmo y tipo    | `{ "alg": "HS256", "typ": "JWT" }`                                          |
| Payload   | Claims (datos)      | `{ "sub": "1234567890", "name": "John Doe", "iat": 1516239022 }`            |
| Signature | Firma digital       | HMACSHA256(base64UrlEncode(header) + "." + base64UrlEncode(payload), clave) |

El header y el payload son objetos JSON codificados en Base64Url. La signature asegura que el token no ha sido alterado.

Ejemplo de payload:

```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "iat": 1516239022
}
```

## Recursos útiles

- [RFC 7519: JSON Web Token (JWT)](https://datatracker.ietf.org/doc/html/rfc7519)
- [JWT Debugger](https://www.jwt.io/)
