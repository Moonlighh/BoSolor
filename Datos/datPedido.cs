using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datPedido
    {
        public static datPedido Instancia { get; } = new();
        private datPedido() { }

        #region CRUD
        
        public bool CrearPedido(PedidoModel entidad)
        {
            // El id del repartidor se asigna solo cuando se toma el pedido por parte del repartidor
            // El estado del pedido se asigna en el procedimiento almacenado (PENDIENTE)
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@direccionEntrega", entidad.DireccionEntrega);
            cmd.Parameters.AddWithValue("@referencia", (object)entidad.Referencia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@detalle", (object)entidad.Detalle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@idCliente", entidad.IdCliente);
            cmd.Parameters.AddWithValue("@idTransaccion", entidad.IdTransaccion);

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
        
        public List<PedidoModel> ListarPedidos()
        {
            var lsPedidos = new List<PedidoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarPedidos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var pedido = new PedidoModel
                    {
                        IdPedido = Convert.ToInt32(dr["idPedido"]),
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        },
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        FechaRecepcion = dr["fechaRecepcion"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntrega = dr["fechaEntrega"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechaEntrega"]),
                        Detalle = dr["detalle"]?.ToString() ?? "",
                        IdRepartidorNavegacion = new CuentaModel
                        {
                            Nombres = dr["repartidor"] == DBNull.Value ? "" : dr["repartidor"].ToString(),
                        },
                        IdEstadoPedidoNavegacion = new EstadoPedidoModel
                        {
                            NombreEstado = dr["nombreEstado"]?.ToString() ?? "",
                        },
                        IdTransaccionNavegacion = new TransaccionModel
                        {
                            CodTransaccion = dr["codTransaccion"]?.ToString() ?? "",
                            MontoBruto = Convert.ToDecimal(dr["montoBruto"]),
                            Descuento = Convert.ToDecimal(dr["descuento"]),
                            MontoTotal = Convert.ToDecimal(dr["montoTotal"]),
                            EstadoTransaccion = dr["estadoTransaccion"]?.ToString() ?? "",
                            Nota = dr["nota"] == DBNull.Value ? "" : dr["nota"].ToString(),
                        },
                    };
                    lsPedidos.Add(pedido);
                }
            }
            catch
            {

                throw;
            }
            return lsPedidos;

        }// Jefe
        #endregion CRUD

        #region Otros
        
        public List<PedidoModel> ListarPedidosPendientes()//Solo repartidor
        {
            var lsPedidos = new List<PedidoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarPedidosPendientes", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var pedido = new PedidoModel
                    {
                        IdPedido = Convert.ToInt32(dr["idPedido"]),
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        },
                        DireccionEntrega = dr["direccionEntrega"]?.ToString() ?? "",
                        Referencia = dr["referencia"]?.ToString() ?? "",
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        Detalle = dr["detalle"]?.ToString() ?? "",
                        IdTransaccionNavegacion = new TransaccionModel
                        {
                            MontoBruto = Convert.ToDecimal(dr["montoBruto"]),
                            Descuento = Convert.ToDecimal(dr["descuento"]),
                            MontoTotal = Convert.ToDecimal(dr["montoTotal"]),
                            EstadoTransaccion = dr["estadoTransaccion"]?.ToString() ?? "",
                        },
                    };
                    lsPedidos.Add(pedido);
                }
            }
            catch
            {

                throw;
            }
            return lsPedidos;

        }
        
        public List<PedidoModel> ListarPedidosRepartidor(int idRepartidor)
        {
            var lsPedidos = new List<PedidoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarPedidosRepartidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRepartidor", idRepartidor);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var pedido = new PedidoModel
                    {
                        IdPedido = Convert.ToInt32(dr["idPedido"]),
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        },
                        DireccionEntrega = dr["direccionEntrega"]?.ToString() ?? "",
                        Referencia = dr["referencia"]?.ToString() ?? "",
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        FechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntrega = dr["fechaEntrega"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechaEntrega"]),
                        Detalle = dr["detalle"]?.ToString() ?? "",
                        IdEstadoPedidoNavegacion = new EstadoPedidoModel
                        {
                            NombreEstado = dr["nombreEstado"]?.ToString() ?? "",
                        },
                        IdTransaccionNavegacion = new TransaccionModel
                        {
                            MontoTotal = Convert.ToDecimal(dr["montoTotal"]),
                            MontoBruto = Convert.ToDecimal(dr["montoBruto"]),
                        },
                    };
                    lsPedidos.Add(pedido);
                }
            }
            catch
            {

                throw;
            }
            return lsPedidos;
        }
        
        public bool TomarPedido(int idPedido, int idRepartidor)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spTomarPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            cmd.Parameters.AddWithValue("@idRepartidor", idRepartidor);
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
        
        public bool ConfirmarEntrega(int idPedido)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spConfirmarEntrega", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
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
        #endregion Otros
    }
}

