using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    [Authorize(Roles = "Jefe")]
    public class TransaccionController : Controller
    {
        public IActionResult Index()
        {
            var listar = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.Todas);
            return View(listar);
        }

        public IActionResult ListarCompras()
        {
            var lsCompra = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.Compra);
            return View(lsCompra);
        }

        public IActionResult ListarVentas()
        {
            var lsVenta = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.Venta);
            return View(lsVenta);
        }

        public IActionResult ListarPedidos()
        {
            var lsPedido = logTransaccion.Instancia.ListarTransacciones(TipoTransaccion.Pedido);
            return View(lsPedido);
        }

        public IActionResult Detalle(int id)
        {
            var lsDetalle = logTransaccion.Instancia.ListarDetalleTransaccion(id);
            return View(lsDetalle);
        }

        public IActionResult ReporteCompras()
        {
            return View();
        }

        public IActionResult ReporteVentas()
        {
            return View();
        }

        public IActionResult ReportePedidos()
        {
            return View();
        }
    }
}
