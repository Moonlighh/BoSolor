using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datCliente
    {
        public static datCliente Instancia { get; } = new();
        private datCliente() { }

        #region CRUD
        
        public bool CrearCliente(ClienteModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearCliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cliente", entidad.Cliente);
            cmd.Parameters.AddWithValue("@numDocumento", entidad.NumDocumento);
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
        
        public List<ClienteModel> ListarClientes()
        {
            var lsCliente = new List<ClienteModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarClientes", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var cliente = new ClienteModel
                    {
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        Cliente = dr["Cliente"]?.ToString() ?? "",
                        NumDocumento = dr["NumDocumento"]?.ToString() ?? "",
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        // Propiedades adicionales para el reporte de clientes
                        CantidadTransacciones = Convert.ToInt32(dr["CantidadTransacciones"])
                    };
                    lsCliente.Add(cliente);
                }
            }
            catch
            {
                throw;
            }
            return lsCliente;
        }
        
        public bool ActualizarCliente(ClienteModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarCliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idCliente", entidad.IdCliente);
            cmd.Parameters.AddWithValue("@nombres", entidad.Cliente);
            cmd.Parameters.AddWithValue("@numDocumento", entidad.NumDocumento);
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
        
        public bool EliminarCliente(int id)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarCliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idCliente", id);
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
