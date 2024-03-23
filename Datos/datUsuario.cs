using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;


namespace BODEGA_SOLORZANO.Datos
{
    public class datUsuario
    {
        public static datUsuario Instancia { get; } = new();
        private datUsuario() { }

        #region Otros
        
        public UsuarioModel IniciarSesion(string usuario, string contra)
        {
            UsuarioModel userEncontrado = null;
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spIniciarSesion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@pass", contra);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    userEncontrado = new UsuarioModel
                    {
                        IdUsuario = Convert.ToInt32(dr["IdCuenta"]),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        Rol = dr["Rol"]?.ToString() ?? "",
                        Usuario = dr["userName"]?.ToString() ?? "",
                        Activo = Convert.ToBoolean(dr["Activo"]),
                    };
                    return userEncontrado;
                }
            }
            catch
            {
                throw;
            }

            return userEncontrado;
        }
        #endregion Otros

    }
}
