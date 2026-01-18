# Subida de Archivos en .NET y Uso de Archivos Estáticos

## Introducción

La subida de archivos en aplicaciones ASP.NET Core es una funcionalidad común que permite a los usuarios cargar imágenes, documentos u otros archivos al servidor. Para servir estos archivos posteriormente, es necesario habilitar el middleware de archivos estáticos.

## Habilitación de Archivos Estáticos

Para que los archivos subidos puedan ser accesibles desde el navegador, se debe agregar la siguiente línea en el archivo `Program.cs`:

```csharp
app.UseStaticFiles(); // Enable static files middleware
```

Esta instrucción permite que el servidor web sirva archivos desde la carpeta `wwwroot` (por defecto) o desde una ruta personalizada configurada.

## Pasos para Subir Archivos

1. **Configurar el Middleware de Archivos Estáticos**

   - Agrega `app.UseStaticFiles();` en el pipeline de la aplicación, normalmente después de la redirección HTTPS y antes de la autorización.

2. **Crear un Endpoint para la Subida de Archivos**

   - En el controlador, utiliza el tipo `IFormFile` para recibir el archivo desde el cliente.
   - Ejemplo:

     ```csharp
     [HttpPost("upload")]
     public async Task<IActionResult> UploadFile(IFormFile file)
     {
         if (file == null || file.Length == 0)
             return BadRequest("No se seleccionó ningún archivo.");

         var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
         using (var stream = new FileStream(path, FileMode.Create))
         {
             await file.CopyToAsync(stream);
         }
         return Ok(new { file.FileName });
     }
     ```

3. **Acceso a los Archivos Subidos**
   - Una vez subido el archivo a la carpeta `wwwroot`, puede ser accedido mediante una URL como:
     ```
     https://tudominio.com/nombre-del-archivo.jpg
     ```

## Ejemplo avanzado: Guardar imágenes de productos

En el archivo `ProductsController.cs` se implementó una lógica para guardar imágenes subidas en una carpeta específica (`wwwroot/ProductsImages`) y construir la URL pública y local del archivo:

```csharp
if(createProductDto.Image != null)
{
    string fileName = product.ProductId + Guid.NewGuid().ToString() + Path.GetExtension(createProductDto.Image.FileName);
    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","ProductsImages");
    if (!Directory.Exists(imagesFolder))
    {
        Directory.CreateDirectory(imagesFolder);
    }

    var filePath = Path.Combine(imagesFolder, fileName);
    FileInfo file = new(filePath);
    if (file.Exists)
    {
        file.Delete();
    }
    using var fileStream = new FileStream(filePath, FileMode.Create); // Crear el archivo
    createProductDto.Image.CopyTo(fileStream); // Copiar la imagen subida al stream
    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}"; // Obtener la URL base de la petición
    product.ImgUrl = $"{baseUrl}/ProductsImages/{fileName}"; // Asignar la URL pública
    product.ImgUrlLocal = filePath; // Asignar la ruta local
} else
{
    product.ImgUrl = "https://placehold.co/600x400";
}
```

**Notas:**

- Se crea la carpeta `ProductsImages` si no existe.
- Se elimina el archivo anterior si existe para evitar duplicados.
- Se genera una URL pública para acceder a la imagen desde el navegador.
- Si no se sube imagen, se asigna una imagen por defecto.

Esta lógica permite gestionar imágenes de productos de forma segura y accesible desde la web.

## Consideraciones de Seguridad

- Valida el tipo y tamaño de los archivos antes de guardarlos.
- Usa rutas seguras y evita sobrescribir archivos existentes sin control.
- Configura reglas de acceso si los archivos no deben ser públicos.

## Referencias

- [Documentación oficial de ASP.NET Core: Working with static files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files)
- [Subida de archivos en ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads)

---

**Nota:** La línea `app.UseStaticFiles();` en `Program.cs` es esencial para servir archivos subidos desde el servidor.
