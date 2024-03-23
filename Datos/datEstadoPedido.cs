using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datEstadoPedido
    {
        public static datEstadoPedido Instancia { get; } = new();
        private datEstadoPedido() { }

        #region R    
        
        public List<EstadoPedidoModel> ListarEstadoPedidos()
        {
            var lsEstadoPedido = new List<EstadoPedidoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarEstadosPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var estadoPedido = new EstadoPedidoModel
                    {
                        IdEstadoPedido = Convert.ToInt32(dr["IdEstadoPedido"]),
                        NombreEstado = dr["NombreEstado"]?.ToString() ?? ""
                    };
                    lsEstadoPedido.Add(estadoPedido);
                }
            }
            catch
            {
                throw;
            }
            return lsEstadoPedido;
        }
        #endregion R

    }
}
