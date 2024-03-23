using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class EstadoPedidoController : Controller
    {
        // Tiene su vista pero no se usa a menos que luego se quiera agregar un estado de pedido o algo asi
        public IActionResult Index()
        {
            var lsEstadosPedido = logEstadoPedido.Instancia.ListarEstadoPedido();
            return View(lsEstadosPedido);
        }
    }
}
