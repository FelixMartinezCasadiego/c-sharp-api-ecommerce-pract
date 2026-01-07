# ¿Qué es un DTO?

Un **DTO** (Data Transfer Object, u Objeto de Transferencia de Datos) es un patrón de diseño utilizado para transportar datos entre diferentes capas de una aplicación, como entre el backend y el frontend, o entre la capa de presentación y la lógica de negocio.

## Representación visual del flujo

El siguiente cuadro muestra cómo el DTO actúa como intermediario entre el modelo de dominio y el cliente:

| Domain Model |  →  | DTO |  →  | Client |
| :----------: | :-: | :-: | :-: | :----: |

Esto significa que los datos del modelo de dominio no se envían directamente al cliente, sino que pasan primero por un DTO, que los transforma y adapta según las necesidades de la comunicación.

## Características principales

- **Simplicidad:** Un DTO solo contiene atributos (propiedades) y no incluye lógica de negocio ni métodos complejos.
- **Transporte de datos:** Su objetivo principal es transferir datos de manera eficiente y segura.
- **Evita exponer entidades:** Ayuda a no exponer directamente las entidades del dominio o los modelos de base de datos.
- **Facilita validaciones:** Permite validar y transformar datos antes de enviarlos o recibirlos.

## Ejemplo de uso

Supón que tienes una entidad `Category` en tu base de datos, pero solo quieres enviar el nombre y la descripción al cliente. Puedes crear un `CategoryDto` con solo esos campos, evitando exponer información sensible o innecesaria.

```csharp
public class CategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
```

## Ventajas de usar DTOs

- **Seguridad:** Controlas exactamente qué datos se exponen.
- **Desacoplamiento:** Reduces el acoplamiento entre capas de la aplicación.
- **Mantenibilidad:** Facilitas el mantenimiento y la evolución de la API.
- **Optimización:** Puedes enviar solo los datos necesarios, optimizando el rendimiento.

## Resumen

Un DTO es una herramienta fundamental para estructurar y proteger el flujo de datos en aplicaciones modernas, especialmente en arquitecturas basadas en APIs y servicios.
