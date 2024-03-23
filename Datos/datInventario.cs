using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datInventario
    {
        public static datInventario Instancia { get; } = new datInventario();
        private datInventario() { }

        #region R
        
        public List<InventarioModel> ListarDetalleInventario()
        {
            var lsInventario = new List<InventarioModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarDetalleInventario", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var inventario = new InventarioModel
                    {
                        IdInventario = Convert.ToInt32(dr["idInventario"]),
                        IdProductoNavegacion = new ProductoModel
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Codigo = dr["codigo"]?.ToString() ?? "",
                            Nombre = dr["nombre"]?.ToString() ?? "",
                            Descripcion = dr["descripcion"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["PrecioPorConjunto"]),
                            PrecioPorUnidad = Convert.ToDecimal(dr["PrecioPorUnidad"]),
                            UnidadesPorConjunto = Convert.ToInt32(dr["UnidadesPorConjunto"]),
                        },
                        CantidadEnCajas = Convert.ToInt32(dr["CantidadEnCajas"]),
                        CantidadEnUnidades = Convert.ToInt32(dr["CantidadEnUnidades"])
                    };
                    lsInventario.Add(inventario);
                }
            }
            catch
            {
                throw;
            }
            return lsInventario;
        }
        
        public bool ActualizarInventario(InventarioModel inventario)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarInventario", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idInventario", inventario.IdInventario);
            cmd.Parameters.AddWithValue("@idProducto", inventario.IdProductoNavegacion.IdProducto);
            cmd.Parameters.AddWithValue("@cantidadEnCajas", inventario.CantidadEnCajas);
            cmd.Parameters.AddWithValue("@cantidadEnUnidades", inventario.CantidadEnUnidades);
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
        #endregion R

        #region OTROS
        
        public List<InventarioModel> MostrarProductosCompra(int idProveedor)
        {
            var lsInventario = new List<InventarioModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spMostrarProductosCompra", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var inventario = new InventarioModel
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        IdProveedor = Convert.ToInt32(dr["IdProveedor"]),// No existe en la tabla Inventario pero si en la consulta
                        IdProductoNavegacion = new ProductoModel
                        {
                            Nombre = dr["Nombre"]?.ToString() ?? "",
                            Descripcion = dr["Descripcion"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["PrecioVenta"]),
                            PrecioPorUnidad = Convert.ToDecimal(dr["PrecioPorUnidad"]),
                            UnidadesPorConjunto = Convert.ToInt32(dr["UnidadesPorConjunto"]),
                        },
                        PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),// No existe en la tabla Inventario pero si en la consulta
                        CantidadEnCajas = Convert.ToInt32(dr["CantidadEnCajas"]),
                        CantidadEnUnidades = Convert.ToInt32(dr["CantidadEnUnidades"])
                    };
                    lsInventario.Add(inventario);
                }
            }
            catch
            {

                throw;
            }
            return lsInventario;
        }
        
        public List<InventarioModel> MostrarProductosVentaPedido()
        {
            var lsInventario = new List<InventarioModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spMostrarProductosVenta", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var inventario = new InventarioModel
                    {
                        IdProductoNavegacion = new ProductoModel
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Codigo = dr["Codigo"]?.ToString() ?? "",
                            Nombre = dr["Nombre"]?.ToString() ?? "",
                            Descripcion = dr["Descripcion"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["PrecioPorConjunto"]),
                            PrecioPorUnidad = Convert.ToDecimal(dr["PrecioPorUnidad"]),
                            UnidadesPorConjunto = Convert.ToInt32(dr["UnidadesPorConjunto"]),
                        },
                        CantidadEnCajas = Convert.ToInt32(dr["CantidadEnCajas"]),
                        CantidadEnUnidades = Convert.ToInt32(dr["CantidadEnUnidades"])
                    };
                    lsInventario.Add(inventario);
                }
            }
            catch
            {
                throw;
            }
            return lsInventario;
        }
        #endregion OTROS

    }
}
