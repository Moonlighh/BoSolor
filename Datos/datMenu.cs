using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datMenu
    {
        public static datMenu Instancia { get; } = new();
        private datMenu() { }

        #region R
        
        public List<MenuModel> ListarMenus(int idRol, string tipo)
        {
            var lsMenu = new List<MenuModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarMenus", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idRol);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var menu = new MenuModel
                    {
                        IdMenu = Convert.ToInt32(dr["IdMenu"]),
                        Nombre = dr["Nombre"]?.ToString() ?? "",
                        Icono = dr["Icono"]?.ToString() ?? "",
                        Controlador = dr["Controlador"]?.ToString() ?? "",
                        PaginaAccion = dr["PaginaAccion"]?.ToString() ?? "",
                        IdMenuPadre = Convert.ToInt32(dr["IdMenuPadre"]),
                    };
                    lsMenu.Add(menu);
                }
            }
            catch
            {
                throw;
            }
            return lsMenu;
        }
        #endregion R

    }
}
