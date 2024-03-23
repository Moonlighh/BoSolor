using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    [Authorize(Roles = "Jefe")]
    public class MetodoPagoController : Controller
    {
        public IActionResult Index()
        {
            var listaMetodo = logMetodoPago.Instancia.ListarMetodosPago();
            return View(listaMetodo);
        }

        [HttpGet]
        public IActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(MetodoPagoModel metodoPago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = logMetodoPago.Instancia.CrearMetodoPago(metodoPago);

                    if (respuesta)
                    {
                        TempData["Mensaje"] = "Método de pago creado correctamente";
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo crear el método de pago";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(metodoPago);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var respuesta = logMetodoPago.Instancia.EliminarMetodoPago(id);
                if (respuesta)
                {
                    TempData["Mensaje"] = "Método de pago eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el método de pago";
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
