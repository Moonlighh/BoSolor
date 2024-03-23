using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SISTEMA_WEB.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string? nombreUsuario = "";
            string? urlFotoUsuario = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["urlFotoUsuario"] = urlFotoUsuario;
            return View();
        }
    }
}
