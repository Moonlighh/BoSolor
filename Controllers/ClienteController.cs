using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        [Authorize(Roles = "Jefe")]
        public IActionResult Index()//Pertenece solo al jefe y no debe permitir registrar y debe permitir ver el detalle de todas las transacciones que se han hecho
        {
            var listaClientes = logCliente.Instancia.ListarClientes();
            return View(listaClientes);
        }

        public IActionResult Listar()
        {
            var listaClientes = logCliente.Instancia.ListarClientes();
            return View(listaClientes);
        }

        [HttpGet]
        public IActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(ClienteModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = await logCliente.Instancia.CrearCliente(modelo);
                    if (respuesta)
                    {
                        TempData["Mensaje"] = "Cliente registrado correctamente";
                    }
                    else
                    {
                        TempData["Error"] = "Ocurrio un error al registrar el cliente";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(modelo);

        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var respuesta = logCliente.Instancia.EliminarCliente(id);
                if (respuesta)
                {
                    TempData["Mensaje"] = "Cliente eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "Ocurrio un error al eliminar el cliente";
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
