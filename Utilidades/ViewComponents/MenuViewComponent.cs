using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SISTEMA_WEB.Utilidades.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            List<MenuModel> listaMenus;

            if (claimUser.Identity.IsAuthenticated)
            {
                string idRol = claimUser.Claims.Where(c => c.Type == ClaimTypes.Anonymous)
                .Select(c => c.Value)
                .SingleOrDefault();
                listaMenus = logMenu.Instancia.MostrarMenu(Convert.ToInt32(idRol));
                string userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).ToString();
                ViewBag.NombreUsuario = userName;
            }
            else
            {
                listaMenus = new List<MenuModel> { };
            }
            return View(listaMenus);

        }
    }
}
