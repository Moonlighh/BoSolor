using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    [Authorize(Roles = "Ventas")]
    public class VentaController : Controller
    {
        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var lsVenta = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.VentasPorEmpleado, idUsuario);
            return View(lsVenta);
        }

        // La lista de productos disponibles se lista desde el InventarioController
        [HttpPost]
        public IActionResult AgregarProductoVenta(int idProducto, int cantidadUnidades, int cantidadPaquetes)
        {
            try
            {
                if (idProducto <= 0 || (cantidadUnidades <= 0 && cantidadPaquetes <= 0))
                {
                    throw new Exception("Revisa los datos enviados e intenta de nuevo.");
                }
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
                logCarrito.Instancia.AgregarCarritoVenta(idProducto, cantidadPaquetes, cantidadUnidades, Convert.ToInt32(idUsuario));
                TempData["Mensaje"] = $"El producto fué agregado con exito. Consulte el carrito para mayor información";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ProductosDisponibles", "Inventario");
        }

        // Se visualiza el carrito de venta
        [HttpGet]
        public IActionResult CarritoVenta()
        {
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string id = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
                var listaProductos = logCarrito.Instancia.ListarCarritoCompraVenta(Convert.ToInt32(id));
                listaProductos ??= new List<CarritoModel>();
                ViewBag.MetodosPago = logMetodoPago.Instancia.ListarMetodosPago();
                return View(listaProductos);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Se inicia la transaccion
        [HttpPost]
        public async Task<IActionResult> IniciarTransaccion(string dni, decimal descuento, string nota, int idMetodoPago)
        {
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                if (claimUser.Identity.IsAuthenticated)
                {
                    var idUsuario = claimUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    var carritoUsuario = logCarrito.Instancia.ListarCarritoCompraVenta(int.Parse(idUsuario));

                    if (carritoUsuario.Count <= 0)
                    {
                        throw new Exception("No hay productos en el carrito");
                    }

                    var objetoAnonimo = new// Se crea un objeto anonimo para enviar los datos (No cambiar ningun valor)
                    {
                        Dni = dni,
                        Descuento = descuento,
                        Nota = nota,
                        IdMetodoPago = idMetodoPago
                    };

                    string respuesta = await logTransaccion.Instancia.CrearTransaccionVenta(carritoUsuario, objetoAnonimo);

                    if (!string.IsNullOrEmpty(respuesta))
                    {
                        logCarrito.Instancia.EliminarProductosPorUsuario(int.Parse(idUsuario));
                    }
                    TempData["Mensaje"] = respuesta;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("CarritoVenta");
        }

        public IActionResult EliminarProductoVenta(int id)
        {
            try
            {
                logCarrito.Instancia.EliminarCarritoCompraVenta(id);
            }
            catch
            {
                TempData["Error"] = "No se pudo eliminar el producto del carrito";
            }
            return RedirectToAction("CarritoVenta");
        }

        public IActionResult Detalle(int id)
        {
            var lsDetalle = logTransaccion.Instancia.ListarDetalleTransaccion(id);
            return View(lsDetalle);
        }

        public IActionResult GenerarFactura()
        {
            return View();
        }
        public IActionResult Reporte()
        {
            return View();
        }
    }
}
