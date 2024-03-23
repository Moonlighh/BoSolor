using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    [Authorize(Roles = "Compras")]
    public class CompraController : Controller
    {
        private readonly List<DetalleTransaccionModel> lsDetalleProducto = new();

        public IActionResult Index()
        {
            var lsCompra = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.Compra);
            return View(lsCompra);
        }

        public IActionResult AgregarProducto(DetalleTransaccionModel detalle)
        {
            lsDetalleProducto.Add(detalle);
            return View();
        }

        #region Proceso de Compra
        // Primero se selecciona el proveedor
        public IActionResult Productos(int idProveedor)
        {
            try
            {
                // Si idProveedor sigue siendo cero, devolver una lista vacía
                if (idProveedor <= 0)
                {
                    var lsVacia = new List<ProveedorProductoModel>();
                    return View(lsVacia);
                }

                // Consultar y devolver la lista de productos del proveedor
                List<ProveedorProductoModel> lsProducto = logProveedorProducto.Instancia.ListarProductosCompra(idProveedor);
                return View(lsProducto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Compra");
            }
        }

        // Luego se agrega el producto
        [HttpPost]
        public IActionResult AgregarProductoCompra(int idProducto, int idProveedor, string nombre, int cantidad, decimal precio)
        {
            try
            {
                if (cantidad <= 0)
                {
                    throw new Exception("La cantidad debe ser mayor a 0.");
                }
                ClaimsPrincipal claimUser = HttpContext.User;
                string id = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
                CarritoModel carrito = new()
                {
                    IdProducto = idProducto,
                    IdProveedor = idProveedor,
                    Producto = nombre,
                    CantidadPaquetes = cantidad,
                    PrecioPaquete = precio,
                    IdUsuario = Convert.ToInt32(id)
                };
                logCarrito.Instancia.AgregarCarritoCompra(carrito);
                TempData["Mensaje"] = $"El producto {carrito.Producto} fué agregado con exito";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Productos", new { idProveedor });
        }

        // Se visualiza el carrito de compra
        [HttpGet]
        public IActionResult CarritoCompra()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string id = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var listaProductos = logCarrito.Instancia.ListarCarritoCompraVenta(Convert.ToInt32(id));
            listaProductos ??= new List<CarritoModel>();
            return View(listaProductos);
        }

        // Se inicia la transaccion
        [HttpPost]
        public IActionResult IniciarTransaccion()
        {
            string mensaje = string.Empty;
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

                    bool respuesta = logTransaccion.Instancia.CrearTransaccionCompra(carritoUsuario, ref mensaje);
                    if (respuesta)
                    {
                        logCarrito.Instancia.EliminarProductosPorUsuario(int.Parse(idUsuario));
                    }
                    TempData["Mensaje"] = mensaje;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("CarritoCompra", "Compra");
        }

        #endregion Proceso de Compra

        public IActionResult EliminarProductoCompra(int id)
        {
            try
            {
                logCarrito.Instancia.EliminarCarritoCompraVenta(id);
            }
            catch
            {
                TempData["Error"] = "No se pudo eliminar el producto del carrito";
            }
            return RedirectToAction("CarritoCompra");
        }

        public IActionResult Detalle(int id)
        {
            var lsDetalle = logTransaccion.Instancia.ListarDetalleTransaccion(id);
            return View(lsDetalle);
        }

        public IActionResult Reporte()
        {
            return View();
        }
    }
}
