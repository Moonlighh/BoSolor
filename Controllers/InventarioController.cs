using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        [Authorize(Roles = "Jefe,Compras,Ventas")]
        public IActionResult Index()
        {
            var lsInventario = logInventario.Instancia.ListarDetalleInventario();
            return View(lsInventario);
        }

        [Authorize(Roles = "Ventas")]
        public IActionResult ProductosDisponibles()
        {
            var lsProductosDisponibles = logInventario.Instancia.MostrarProductosVentaPedido();
            return View(lsProductosDisponibles);
        }

        [Authorize(Roles = "Ventas")]
        public IActionResult AbrirPaquete(int id)
        {
            try
            {
                var paqueteAbierto = logInventario.Instancia.AbrirPaquete(id);
                if (paqueteAbierto)
                {
                    TempData["Success"] = $"Se abrio el paquete correctamente. -> ID PRODUCTO{id}";
                }
                else
                {
                    TempData["Error"] = $"No se pudo abrir el paquete. -> ID PRODUCTO{id}";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = $"Se produjo un error al intentar abrir el paquete. -> ID PRODUCTO{id}";
            }
            return RedirectToAction("ProductosDisponibles");
        }
    }
}
