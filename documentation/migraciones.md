# Documentación: Migraciones en C# con Entity Framework Core

## ¿Qué es una migración?

Una **migración** es un conjunto de instrucciones que permiten actualizar la estructura de la base de datos (tablas, columnas, relaciones, etc.) para que refleje los cambios realizados en el modelo de datos de la aplicación (clases y entidades en C#). Las migraciones ayudan a mantener sincronizados el código y la base de datos de forma controlada y reproducible.

## ¿Para qué se usan las migraciones?

- **Control de versiones de la base de datos**: Permiten llevar un historial de los cambios en la estructura de la base de datos.
- **Automatización**: Facilitan la actualización automática de la base de datos sin tener que escribir manualmente scripts SQL.
- **Trabajo en equipo**: Permiten que varios desarrolladores trabajen sobre el mismo proyecto y mantengan la base de datos sincronizada.

## ¿Cómo crear una migración?

Para crear una migración, se utiliza el siguiente comando en la terminal, ubicado en la raíz del proyecto:

```
dotnet ef migrations add NombreDeLaMigracion
```

- **NombreDeLaMigracion**: Es un nombre descriptivo para la migración, por ejemplo: `AgregarTablaProductos`.

Este comando genera una nueva carpeta (si no existe) llamada `Migrations` y dentro de ella un archivo con las instrucciones para aplicar y revertir los cambios en la base de datos.

## ¿Cómo aplicar una migración a la base de datos?

Después de crear la migración, se debe actualizar la base de datos con el siguiente comando:

```
dotnet ef database update
```

Este comando aplica todas las migraciones pendientes a la base de datos configurada en el proyecto.

## Resumen de comandos principales

- **Crear migración:**
  ```
  dotnet ef migrations add NombreDeLaMigracion
  ```
- **Actualizar base de datos:**
  ```
  dotnet ef database update
  ```

## Motivos para usar migraciones

- Mantener la base de datos alineada con el modelo de datos.
- Evitar errores manuales al modificar la base de datos.
- Facilitar el despliegue y la colaboración en equipos de desarrollo.

---

**Referencia:** [Documentación oficial de EF Core](https://learn.microsoft.com/es-es/ef/core/managing-schemas/migrations/)
