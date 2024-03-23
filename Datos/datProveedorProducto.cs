using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datProveedorProducto
    {
        public static datProveedorProducto Instancia { get; } = new();
        private datProveedorProducto() { }

        #region CRU
        
        public bool CrearProveedorProducto(ProveedorProductoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearProveedorProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProveedor", entidad.IdProveedorNavegacion.IdProveedor);
            cmd.Parameters.AddWithValue("@idProducto", entidad.IdProductoNavegacion.IdProducto);
            cmd.Parameters.AddWithValue("@precioCompra", entidad.PrecioCompra);
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
        
        public List<ProveedorProductoModel> ListarProveedorProductos()
        {
            var lsProveedorProducto = new List<ProveedorProductoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarProveedorProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var proprod = new ProveedorProductoModel
                    {
                        IdProductoNavegacion = new ProductoModel
                        {
                            Nombre = dr["producto"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["precioVenta"]),
                        },
                        IdProveedorNavegacion = new ProveedorModel
                        {
                            RazonSocial = dr["proveedor"]?.ToString() ?? "",
                            Contacto = dr["contacto"]?.ToString() ?? "",
                        },
                        PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                    };
                    lsProveedorProducto.Add(proprod);
                }
            }
            catch
            {
                throw;
            }
            return lsProveedorProducto;
        }
        
        public bool ActualizarProveedorProducto(ProveedorProductoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarProveedorProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProveedorProducto", entidad.IdProveedorProducto);
            cmd.Parameters.AddWithValue("@precioCompra", entidad.PrecioCompra);
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
        #endregion CRU

        #region Otros
        
        public List<ProveedorProductoModel> ListarProveedorProductoPorProveedor(int idProveedor)
        {
            var lsProveedorProducto = new List<ProveedorProductoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarProveedorProductoPorProveedor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var proprod = new ProveedorProductoModel
                    {
                        IdProductoNavegacion = new ProductoModel
                        {
                            IdProducto = Convert.ToInt32(dr["idProducto"]),
                            Nombre = dr["producto"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["precioVenta"]),
                            PrecioPorUnidad = Convert.ToDecimal(dr["precioVentaUnidad"]),
                            UnidadesPorConjunto = Convert.ToInt32(dr["unidadesPorConjunto"]),
                        },
                        IdProveedorNavegacion = new ProveedorModel
                        {
                            IdProveedor = Convert.ToInt32(dr["idProveedor"]),
                        },
                        PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                    };
                    lsProveedorProducto.Add(proprod);
                }
            }
            catch
            {
                throw;
            }
            return lsProveedorProducto;
        }
        #endregion Otros

    }
}
