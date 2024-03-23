using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logMenu
    {
        public static logMenu Instancia { get; } = new();
        private logMenu() { }

        #region CRUD
        public List<MenuModel> MostrarMenu(int numRol)
        {
            List<MenuModel> lsMenuPadre = datMenu.Instancia.ListarMenus(numRol, "PADRE");
            List<MenuModel> lsMenuHijo = datMenu.Instancia.ListarMenus(numRol, "HIJO");
            List<MenuModel> lista = new();

            foreach (var item in lsMenuPadre)
            {
                MenuModel menu = new()
                {
                    IdMenu = item.IdMenu,
                    Nombre = item.Nombre,
                    Icono = item.Icono,
                    Controlador = item.Controlador,
                    PaginaAccion = item.PaginaAccion,
                    IdMenuPadre = item.IdMenuPadre,
                    ListaMenu = new List<MenuModel>()
                };

                foreach (var itemHijo in lsMenuHijo)
                {
                    if (item.IdMenu == itemHijo.IdMenuPadre)
                    {
                        MenuModel menuHijo = new()
                        {
                            IdMenu = itemHijo.IdMenu,
                            Nombre = itemHijo.Nombre,
                            Icono = itemHijo.Icono,
                            Controlador = itemHijo.Controlador,
                            PaginaAccion = itemHijo.PaginaAccion,
                            IdMenuPadre = itemHijo.IdMenuPadre
                        };
                        menu.ListaMenu.Add(menuHijo);
                    }
                }
                lista.Add(menu);
            }

            return lista;
        }
        #endregion CRUD
    }
}
