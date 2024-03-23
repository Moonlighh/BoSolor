using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IWebHostEnvironment _environment;//Obtener la ruta del directorio de contenido web
        public ProductoController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            var listaCategorias = logProducto.Instancia.ListarProductos();
            return View(listaCategorias);
        }

        [HttpPost]
        public IActionResult Guardar(ProductoModel modelo, IFormFile RutaImagen)
        {
            // Falta volver a probar esto
            try
            {
                if (ModelState.IsValid)
                {
                    string nuevoNombreDeArchivo = "";
                    if (RutaImagen != null && RutaImagen.Length > 0)
                    {
                        string extensionDeArchivo = Path.GetExtension(RutaImagen.FileName);
                        nuevoNombreDeArchivo = $"{modelo.Nombre}" + extensionDeArchivo;
                    }
                    modelo.Imagen = "/Productos/Img/" + nuevoNombreDeArchivo;

                    var respuesta = logProducto.Instancia.CrearProducto(modelo);

                    if (respuesta)
                    {
                        if (RutaImagen != null && RutaImagen.Length > 0)
                        {
                            var rutaDeGuardado = Path.Combine(_environment.WebRootPath, "Productos/Img", nuevoNombreDeArchivo);

                            using (var image = Image.FromStream(RutaImagen.OpenReadStream()))
                            {
                                var scaleFactor = Math.Min(200.0 / image.Width, 200.0 / image.Height);
                                var newWidth = (int)(image.Width * scaleFactor);
                                var newHeight = (int)(image.Height * scaleFactor);

                                using var newImage = new Bitmap(image, new Size(newWidth, newHeight));
                                using var stream = new FileStream(rutaDeGuardado, FileMode.Create);
                                newImage.Save(stream, ImageFormat.Jpeg);
                            }

                        }

                        TempData["Mensaje"] = "Producto creado correctamente";
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo crear el producto";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(modelo);
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                var respuesta = logProducto.Instancia.EliminarProducto(id);
                if (respuesta)
                {
                    TempData["Mensaje"] = "Producto eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el producto";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

    }
}
