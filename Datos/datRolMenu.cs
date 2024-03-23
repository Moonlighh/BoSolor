using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datRolMenu
    {
        public static datRolMenu Instancia { get; } = new();
        private datRolMenu() { }

        #region R
        
        public List<RolMenuModel> ListarRolMenus()
        {
            var lsRolMenu = new List<RolMenuModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarRolMenu", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var rolMenu = new RolMenuModel
                    {
                        IdRolMenu = Convert.ToInt32(dr["IdRolMenu"]),
                        IdRolNavegacion = new RolModel
                        {
                            Nombre = dr["Rol"]?.ToString() ?? "",
                            Activo = Convert.ToBoolean(dr["Activo"]),
                        },
                        IdMenuNavegacion = new MenuModel
                        {
                            Nombre = dr["Menu"]?.ToString() ?? "",
                            Controlador = dr["Controlador"]?.ToString() ?? "",
                            PaginaAccion = dr["PaginaAccion"]?.ToString() ?? "",
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                            IdMenuPadre = Convert.ToInt32(dr["IdMenuPadre"]),
                            IdMenuPadreNavegacion = new MenuModel
                            {
                                Nombre = dr["MenuPadre"]?.ToString() ?? "",
                            },
                        },
                    };
                    lsRolMenu.Add(rolMenu);
                }
            }
            catch
            {
                throw;
            }
            return lsRolMenu;
        }
        #endregion R

    }
}