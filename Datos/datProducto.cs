using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datProducto
    {
        public static datProducto Instancia { get; } = new();
        private datProducto() { }

        #region CRUD
        
        public bool CrearProducto(ProductoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object)entidad.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@codigo", entidad.Codigo);
            cmd.Parameters.AddWithValue("@precioPorUnidad", entidad.PrecioPorUnidad);
            cmd.Parameters.AddWithValue("@precioPorConjunto", entidad.PrecioPorConjunto);
            cmd.Parameters.AddWithValue("@unidadesPorConjunto", entidad.UnidadesPorConjunto);
            cmd.Parameters.AddWithValue("@imagen", entidad.Imagen);
            cmd.Parameters.AddWithValue("@idCategoria", entidad.IdCategoria);

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

        
        public List<ProductoModel> ListarProductos()
        {
            var lista = new List<ProductoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarProductos", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var Prod = new ProductoModel
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        Codigo = dr["Codigo"]?.ToString() ?? "",
                        Nombre = dr["Nombre"]?.ToString() ?? "",
                        Descripcion = dr["Descripcion"]?.ToString() ?? "",
                        PrecioPorUnidad = Convert.ToDecimal(dr["PrecioPorUnidad"]),
                        PrecioPorConjunto = Convert.ToDecimal(dr["PrecioPorConjunto"]),
                        UnidadesPorConjunto = Convert.ToInt32(dr["UnidadesPorConjunto"]),
                        Imagen = dr["Imagen"]?.ToString() ?? "",
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        IdCategoriaNavegacion = new CategoriaModel
                        {
                            Nombre = dr["categoria"]?.ToString() ?? ""
                        }
                    };
                    lista.Add(Prod);
                }
            }
            catch
            {
                throw;
            }

            return lista;
        }

        
        public bool ActualizarProducto(ProductoModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spActualizarProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProducto", entidad.IdProducto);
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
            cmd.Parameters.AddWithValue("@codigo", entidad.Codigo);
            cmd.Parameters.AddWithValue("@precioPorUnidad", entidad.PrecioPorUnidad);
            cmd.Parameters.AddWithValue("@precioPorConjunto", entidad.PrecioPorConjunto);
            cmd.Parameters.AddWithValue("@unidadesPorConjunto", entidad.UnidadesPorConjunto);
            cmd.Parameters.AddWithValue("@imagen", entidad.Imagen);
            cmd.Parameters.AddWithValue("@idCategoria", entidad.IdCategoria);
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

        
        public bool EliminarProducto(int id)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProducto", id);
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

        #region OTROS
        public List<ProductoModel> BuscarProducto(string busqueda)
        {
            var lista = new List<ProductoModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spBuscarProducto", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@busqueda", busqueda);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var Prod = new ProductoModel
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        Codigo = dr["Codigo"]?.ToString() ?? "",
                        Nombre = dr["Nombre"]?.ToString() ?? "",
                        Descripcion = dr["Descripcion"]?.ToString() ?? "",
                        PrecioPorUnidad = Convert.ToDecimal(dr["PrecioPorUnidad"]),
                        PrecioPorConjunto = Convert.ToDecimal(dr["PrecioPorConjunto"]),
                        UnidadesPorConjunto = Convert.ToInt32(dr["UnidadesPorConjunto"]),
                        Imagen = dr["Imagen"]?.ToString() ?? "",
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        IdCategoriaNavegacion = new CategoriaModel
                        {
                            Nombre = dr["categoria"]?.ToString() ?? ""
                        }
                    };
                    lista.Add(Prod);
                }
            }
            catch
            {
                throw;
            }
            return lista;
        }
        #endregion OTROS

    }
}
