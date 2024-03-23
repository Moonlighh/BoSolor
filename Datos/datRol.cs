using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datRol
    {
        public static datRol Instancia { get; } = new();
        private datRol() { }

        #region CRUD
        
        public bool CrearRol(RolModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearRol", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
            try
            {
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
        
        public List<RolModel> ListarRoles()
        {
            var lsRol = new List<RolModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarRoles", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var rol = new RolModel
                    {
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        Nombre = dr["Nombre"]?.ToString() ?? "",
                        Descripcion = dr["Descripcion"]?.ToString() ?? "",
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                    };
                    lsRol.Add(rol);
                }
            }
            catch
            {
                throw;
            }
            return lsRol;
        }
        
        public bool ActualizarRol(RolModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarRol", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", entidad.IdRol);
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
            cmd.Parameters.AddWithValue("@activo", entidad.Activo);
            try
            {
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
        
        public bool EliminarRol(int idRol)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarRol", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idRol);
            try
            {
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
        #endregion CRUD

    }
}

