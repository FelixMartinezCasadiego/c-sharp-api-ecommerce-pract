# Instalación de paquetes .NET

En proyectos .NET, los paquetes permiten agregar funcionalidades externas (librerías) de manera sencilla. Existen dos formas principales de instalar paquetes: usando la consola con el comando `dotnet add package` y utilizando la extensión NuGet en Visual Studio Code.

---

## 1. Instalación con la consola (`dotnet add package`)

Puedes instalar paquetes desde la terminal usando el siguiente comando:

```bash
dotnet add package <NombreDelPaquete>
```

Por ejemplo, para instalar AutoMapper:

```bash
dotnet add package AutoMapper
```

Este comando descarga e instala el paquete en el proyecto actual, actualizando el archivo `.csproj` automáticamente.

---

## 2. Instalación usando NuGet en Visual Studio Code

Visual Studio Code permite instalar paquetes NuGet mediante extensiones como **NuGet Package Manager**.

### Pasos:

1. Instala la extensión "NuGet Package Manager" desde el marketplace de VS Code.
2. Haz clic derecho sobre el archivo `.csproj` de tu proyecto en el explorador de archivos.
3. Selecciona la opción "Manage NuGet Packages".
4. Busca el paquete que deseas instalar y haz clic en "Install".

Esto agregará el paquete al proyecto y actualizará el archivo `.csproj`.

---

## Resumen

- Usa la terminal para instalar rápidamente paquetes con `dotnet add package`.
- Usa la extensión NuGet en VS Code para una experiencia visual y gestión avanzada de paquetes.

Ambos métodos son válidos y puedes elegir el que mejor se adapte a tu flujo de trabajo.
