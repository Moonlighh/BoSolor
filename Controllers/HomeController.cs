using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Models.ViewModel;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BODEGA_SOLORZANO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Excep()
        {
            return View();
        }

        #region Vistas
        public ActionResult SinPermisos()
        {
            ViewBag.Message = "Usted no tiene permisos para acceder a esta pagina";
            return View();
        }

        public IActionResult Principal()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Principal");
            }
            return View();
        }

        #endregion Vistas

        #region Acceso, Crear cuenta
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificarAcceso(string user, string pass)
        {
            try
            {
                UsuarioModel objCliente = logUsuario.Instancia.IniciarSesion(user, pass);
                if (objCliente != null)
                {
                    var claims = new List<Claim> // Guardar los datos del usuario un listado de claims
                    {
                        new Claim(ClaimTypes.Name, objCliente.Usuario),
                        new Claim(ClaimTypes.NameIdentifier, objCliente.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Anonymous, objCliente.IdRol.ToString()),
                        new Claim(ClaimTypes.Role, objCliente.Rol),
                    };
                    var claims_identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var properties = new AuthenticationProperties
                    {
                        AllowRefresh = true, // Permitir refrescar el ticket de autenticación
                        ExpiresUtc = DateTime.UtcNow.AddDays(1) // Tiempo de expiración de la cookie
                    };

                    // Registrar la cookie de autenticación en el navegador del usuario

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claims_identity),
                        properties);
                    return RedirectToAction("Principal");
                }
                else
                {
                    TempData["Restablecer"] = "¿Olvidaste tu contraseña?";
                    TempData["Mensaje"] = "Usuario o contraseña incorrectos";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error");
            }
        }
        #endregion

        #region Otros  
        public ActionResult RestablecerPassword()
        {
            return View();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        #endregion
    }
}
