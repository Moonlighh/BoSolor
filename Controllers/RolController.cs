using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    [Authorize(Roles = "Administrador de Sistema")]
    public class RolController : Controller
    {
        public ActionResult Index()
        {
            var lsRol = logRol.Instancia.ListarRol();
            return View(lsRol);
        }

        public IActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guardar(RolModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = logRol.Instancia.CrearRol(modelo);

                    if (respuesta)
                    {
                        TempData["Mensaje"] = "Rol creado correctamente";
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo crear el rol";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(modelo);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
