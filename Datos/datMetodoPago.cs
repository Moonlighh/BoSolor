using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datMetodoPago
    {
        public static datMetodoPago Instancia { get; } = new();
        private datMetodoPago() { }

        #region CRUD
        
        public bool CrearMetodoPago(MetodoPagoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearMetodoPago", cn);
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
        
        public List<MetodoPagoModel> ListarMetodosPago()
        {
            var lsMetodoPago = new List<MetodoPagoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarMetodosPago", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var metodoPago = new MetodoPagoModel
                    {
                        IdMetodo = Convert.ToInt32(dr["IdMetodo"]),
                        Nombre = dr["Nombre"]?.ToString() ?? "",
                        Descripcion = dr["Descripcion"]?.ToString() ?? "",
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                    };
                    lsMetodoPago.Add(metodoPago);
                }
            }
            catch
            {
                throw;
            }
            return lsMetodoPago;
        }
        
        public bool ActualizarMetodoPago(MetodoPagoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarMetodoPago", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idMetodo", entidad.IdMetodo);
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
        
        public bool EliminarMetodoPago(int id)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarMetodoPago", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idMetodo", id);
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
