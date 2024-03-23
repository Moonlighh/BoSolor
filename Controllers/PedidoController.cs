using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Models.ViewModel;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        #region Vendedor
        [Authorize(Roles = "Ventas")]
        [HttpGet]
        public IActionResult Index()
        {
            var listaPedidos = logPedido.Instancia.ListarPedidos();
            return View(listaPedidos);
        }

        [Authorize(Roles = "Ventas")]
        [HttpGet]
        public IActionResult ProductosDisponibles()
        {
            var lsProductosDisponibles = logInventario.Instancia.MostrarProductosVentaPedido();
            return View(lsProductosDisponibles);
        }

        [Authorize(Roles = "Ventas")]
        [HttpPost]
        public IActionResult AgregarProductoPedido(int idProducto, int cantidadPaquetes)
        {
            try
            {
                if (idProducto <= 0 || cantidadPaquetes <= 0)
                {
                    throw new Exception("Revisa los datos enviados e intenta de nuevo.");
                }
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
                logCarrito.Instancia.AgregarCarritoPedido(idProducto, cantidadPaquetes, Convert.ToInt32(idUsuario));
                TempData["Mensaje"] = $"El producto fué agregado al pedido. Consulte el carrito para mayor información";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ProductosDisponibles", "Pedido");
        }

        #region Carrito de pedido
        [Authorize(Roles = "Ventas")]
        [HttpGet]
        public IActionResult CarritoPedido()
        {
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string id = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
                var listaProductos = logCarrito.Instancia.ListarCarritoPedido(Convert.ToInt32(id));
                listaProductos ??= new List<CarritoModel>();
                ViewBag.MetodosPago = logMetodoPago.Instancia.ListarMetodosPago();
                return View(listaProductos);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize(Roles = "Ventas")]
        [HttpDelete]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                logCarrito.Instancia.EliminarCarritoPedido(id);                
            }
            catch
            {
                TempData["Error"] = "No se pudo eliminar el producto del carrito";
            }
            return RedirectToAction("CarritoPedido");
        }

        [Authorize(Roles = "Ventas")]
        [HttpPost]
        public async Task<IActionResult> RegistrarPedido(PedidoViewModel modelo)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    ClaimsPrincipal claimUser = HttpContext.User;
                    if (claimUser.Identity.IsAuthenticated)
                    {
                        var idUsuario = claimUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                        var carritoUsuario = logCarrito.Instancia.ListarCarritoPedido(int.Parse(idUsuario));

                        if (carritoUsuario.Count <= 0)
                        {
                            throw new Exception("No hay productos en el carrito");
                        }
                        if (modelo.IdMetodoPago <= 0)
                        {
                            throw new Exception("El metodo de pago es requerido");
                        }

                        string respuesta = await logTransaccion.Instancia.CrearTransaccionPedido(carritoUsuario, modelo);

                        if (!string.IsNullOrEmpty(respuesta))
                        {
                            logCarrito.Instancia.EliminarProductosPorUsuarioPedido(int.Parse(idUsuario));
                        }
                        TempData["Mensaje"] = respuesta;
                    }
                //}
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("CarritoPedido");
        }

        [Authorize(Roles = "Ventas")]
        [HttpPost]
        public IActionResult ConfirmarPago(string cod, int id)
        {
            try
            {
                bool confirmado = logTransaccion.Instancia.ConfirmarPagoPedido(cod, id);
                if (confirmado)
                {
                    TempData["Mensaje"] = "Se confirmó el pago del pedido exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo confirmar el pago del pedido";
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Se produjo un error al intentar realizar la solicitud";
            }
            return RedirectToAction("Index");
        }

        #endregion Carrito de pedido

        #endregion Vendedor

        #region Repartidor
        [Authorize(Roles = "Repartidor")]
        public IActionResult Pendientes()
        {
            var lsPedidosPendientes= logPedido.Instancia.ListarPedidosPendientes();
            return View(lsPedidosPendientes);
        }

        [Authorize(Roles = "Repartidor")]
        [HttpPost]
        public IActionResult TomarPedido(int id)
        {
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idRepartidor = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

                bool tomado = logPedido.Instancia.TomarPedido(id, Convert.ToInt16(idRepartidor));
                if (tomado)
                {
                    TempData["Mensaje"] = "Pedido tomado correctamente. Consulta tus pedidos asignados";
                }
                else
                {
                    TempData["Error"] = "No se pudo tomar el pedido";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Pendientes");
        }
        
        [Authorize(Roles = "Repartidor")]
        [HttpGet]
        public IActionResult MisPedidos()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string idRepartidor = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var lsPedidos = logPedido.Instancia.ListarPedidosPorRepartidor(Convert.ToInt16(idRepartidor));

            return View(lsPedidos);
        }
        
        [Authorize(Roles = "Repartidor")]
        [HttpPost]
        public IActionResult ConfirmarEntrega(int id)
        {
            try
            {
                bool tomado = logPedido.Instancia.ConfirmarEntrega(id);
                if (tomado)
                {
                    TempData["Mensaje"] = "Se confirmó la entrega del pedido exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("MisPedidos");
        }

        [Authorize(Roles = "Repartidor")]
        [HttpGet]
        public IActionResult DetallePedido(int id)
        {
            var lsDetallePedido = logTransaccion.Instancia.ListarDetalleTransaccionPedido(id);
            return View(lsDetallePedido);
        }
        #endregion Repartidor
    }
}
