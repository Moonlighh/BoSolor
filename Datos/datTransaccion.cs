using BODEGA_SOLORZANO.Models.BoSolor;
using System.Data;
using System.Data.SqlClient;

namespace BODEGA_SOLORZANO.Datos
{
    public class datTransaccion
    {
        public static datTransaccion Instancia { get; } = new();
        private datTransaccion() { }

        #region CRUD
        
        public int IniciarTransaccion(TransaccionModel entidad)
        {
            int idTransaccion = 0;
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spIniciarTransaccion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codTransaccion", entidad.CodTransaccion);
            cmd.Parameters.AddWithValue("@tipoTransaccion", entidad.TipoTransaccion);
            cmd.Parameters.AddWithValue("@nota", (object)entidad.Nota ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@montoBruto", entidad.MontoBruto);
            cmd.Parameters.AddWithValue("@descuento", entidad.Descuento);
            cmd.Parameters.AddWithValue("@montoTotal", entidad.MontoTotal);
            cmd.Parameters.AddWithValue("@estadoTransaccion", entidad.EstadoTransaccion);
            cmd.Parameters.AddWithValue("@idPersona", entidad.IdPersona);
            cmd.Parameters.AddWithValue("@idMetodo", entidad.IdMetodo);
            cmd.Parameters.AddWithValue("@idCuenta", entidad.IdCuenta);
            try
            {
                SqlParameter paramIdTransaccion = new("@idTransaccion", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramIdTransaccion);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    idTransaccion = Convert.ToInt32(paramIdTransaccion.Value);
                }
            }
            catch
            {
                throw;
            }

            return idTransaccion;
        }
        
        public List<TransaccionModel> ListarTransacciones()
        {
            var lsTransaccion = new List<TransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarTransacciones", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var transaccion = new TransaccionModel
                    {
                        IdTransaccion = Convert.ToInt32(dr["IdTransaccion"]),
                        TipoTransaccion = dr["TipoTransaccion"]?.ToString() ?? "",
                        Nota = dr["Nota"]?.ToString() ?? "",
                        CodTransaccion = dr["CodTransaccion"]?.ToString() ?? "",
                        IdMetodoNavegacion = new MetodoPagoModel
                        {
                            Nombre = dr["metodo"]?.ToString() ?? "",
                        },
                        MontoBruto = Convert.ToDecimal(dr["MontoBruto"]),
                        Descuento = Convert.ToDecimal(dr["Descuento"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        FechaTransaccion = Convert.ToDateTime(dr["FechaTransaccion"]),
                        EstadoTransaccion = dr["EstadoTransaccion"]?.ToString() ?? "",
                        IdCuentaNavegacion = new CuentaModel
                        {
                            Nombres = dr["usuario"]?.ToString() ?? "",
                        }
                    };
                    lsTransaccion.Add(transaccion);
                }
            }
            catch
            {

                throw;
            }
            return lsTransaccion;
        }
        
        public List<TransaccionModel> ListarTransaccionCompra()
        {
            var lsTransaccion = new List<TransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarTransaccionCompra", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var transaccion = new TransaccionModel
                    {
                        IdTransaccion = Convert.ToInt32(dr["IdTransaccion"]),
                        CodTransaccion = dr["CodTransaccion"]?.ToString() ?? "",
                        IdMetodoNavegacion = new MetodoPagoModel
                        {
                            Nombre = dr["metodo"]?.ToString() ?? "",
                        },
                        MontoBruto = Convert.ToDecimal(dr["MontoBruto"]),
                        Descuento = Convert.ToDecimal(dr["Descuento"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        FechaTransaccion = Convert.ToDateTime(dr["FechaTransaccion"]),
                        EstadoTransaccion = dr["EstadoTransaccion"]?.ToString() ?? "",
                        IdCuentaNavegacion = new CuentaModel
                        {
                            Nombres = dr["usuario"]?.ToString() ?? "",
                        },
                        IdProveedorNavegacion = new ProveedorModel
                        {
                            RazonSocial = dr["proveedor"]?.ToString() ?? "",
                        }

                    };
                    lsTransaccion.Add(transaccion);
                }

            }
            catch
            {

                throw;
            }
            return lsTransaccion;

        }
        
        public List<TransaccionModel> ListarTransaccionVenta()
        {
            var lsTransaccion = new List<TransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarTransaccionVenta", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var transaccion = new TransaccionModel
                    {
                        IdTransaccion = Convert.ToInt32(dr["IdTransaccion"]),
                        CodTransaccion = dr["CodTransaccion"]?.ToString() ?? "",
                        IdMetodoNavegacion = new MetodoPagoModel
                        {
                            Nombre = dr["metodo"]?.ToString() ?? "",
                        },
                        MontoBruto = Convert.ToDecimal(dr["MontoBruto"]),
                        Descuento = Convert.ToDecimal(dr["Descuento"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        FechaTransaccion = Convert.ToDateTime(dr["FechaTransaccion"]),
                        EstadoTransaccion = dr["EstadoTransaccion"]?.ToString() ?? "",
                        IdCuentaNavegacion = new CuentaModel
                        {
                            Nombres = dr["usuario"]?.ToString() ?? "",
                        },
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        }
                    };
                    lsTransaccion.Add(transaccion);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return lsTransaccion;
        }
        
        public List<TransaccionModel> ListarTransaccionPedido()
        {
            var lsTransaccion = new List<TransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarTransaccionPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var transaccion = new TransaccionModel
                    {
                        IdTransaccion = Convert.ToInt32(dr["IdTransaccion"]),
                        CodTransaccion = dr["CodTransaccion"]?.ToString() ?? "",
                        Nota = dr["Nota"]?.ToString() ?? "",
                        IdMetodoNavegacion = new MetodoPagoModel
                        {
                            Nombre = dr["metodo"]?.ToString() ?? "",
                        },
                        MontoBruto = Convert.ToDecimal(dr["MontoBruto"]),
                        Descuento = Convert.ToDecimal(dr["Descuento"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        FechaTransaccion = Convert.ToDateTime(dr["FechaTransaccion"]),
                        EstadoTransaccion = dr["EstadoTransaccion"]?.ToString() ?? "",
                        IdCuentaNavegacion = new CuentaModel
                        {
                            Nombres = dr["usuario"]?.ToString() ?? "",
                        },
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        }
                    };
                    lsTransaccion.Add(transaccion);
                }
            }
            catch
            {

                throw;
            }
            return lsTransaccion;

        }
        #endregion CRUD

        #region Detalle
        
        public bool CrearDetalleTransaccion(DetalleTransaccionModel entidad)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spCrearDetalleTransaccion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cantidadPorConjunto", entidad.CantidadPorConjunto);
            cmd.Parameters.AddWithValue("@cantidadUnidades", entidad.CantidadUnidadades);
            cmd.Parameters.AddWithValue("@subTotal", entidad.SubTotal);
            cmd.Parameters.AddWithValue("@idTransaccion", entidad.IdTransaccion);
            cmd.Parameters.AddWithValue("@idProducto", entidad.IdProducto);
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
        
        public List<DetalleTransaccionModel> ListarDetalleTransaccion(int id)
        {
            var lsDetalleCompra = new List<DetalleTransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarDetalleTransaccion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idTransaccion", id);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var detalle = new DetalleTransaccionModel
                    {
                        CantidadPorConjunto = Convert.ToInt16(dr["cantidadPorConjunto"]),
                        CantidadUnidadades = Convert.ToInt16(dr["cantidadUnidades"]),
                        SubTotal = Convert.ToDecimal(dr["subTotal"]),
                        IdProductoNavegacion = new ProductoModel
                        {
                            Nombre = dr["nombre"]?.ToString() ?? "",
                            Descripcion = dr["descripcion"]?.ToString() ?? "",
                            IdCategoriaNavegacion = new CategoriaModel
                            {
                                Nombre = dr["categoria"]?.ToString() ?? "",
                            },
                        },
                    };
                    lsDetalleCompra.Add(detalle);
                }
            }
            catch
            {
                throw;
            }
            return lsDetalleCompra;
        }
        
        public List<DetalleTransaccionModel> ListarDetalleTransaccionPedido(int idPedido)
        {
            var lsDetallePedido = new List<DetalleTransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarDetalleTransaccionPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var detalleTransaccion = new DetalleTransaccionModel
                    {
                        IdDetalleTransaccion = Convert.ToInt32(dr["idDetalleTransaccion"]),
                        CantidadPorConjunto = Convert.ToInt16(dr["paquetes"]),
                        SubTotal = Convert.ToDecimal(dr["subTotal"]),
                        IdProductoNavegacion = new ProductoModel
                        {
                            Nombre = dr["nombre"]?.ToString() ?? "",
                            Descripcion = dr["descripcion"]?.ToString() ?? "",
                            Codigo = dr["codigo"]?.ToString() ?? "",
                            PrecioPorConjunto = Convert.ToDecimal(dr["precio"]),
                            Imagen = dr["imagen"]?.ToString() ?? "",
                            IdCategoriaNavegacion = new CategoriaModel
                            {
                                Nombre = dr["categoria"]?.ToString() ?? "",
                            },
                        },
                    };
                    lsDetallePedido.Add(detalleTransaccion);
                }
            }
            catch
            {

                throw;
            }
            return lsDetallePedido;
        }
        
        public bool EliminarDetalleTransaccion(int idTransaccion)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarDetalleTransaccion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idTransaccion", idTransaccion);
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
        #endregion Detalle

        #region Otros
        
        public bool ActualizarEstadoTransaccionYPedido(int idTransaccion, int idPedido)
        {
            /*
             * Este metodo solo sirve para actualizar el estado de la transaccion a pagado si es que
             * el vendedor confirma la recepcion del dinero, y tambien actualiza el estado del pedido
             * a pagado cuando la transaccion es exitosa.
             * 
             */
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spConfirmarPagoPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idTransaccion", idTransaccion);
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            try
            {
                SqlParameter paramConfirmado = new("@confirmado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(paramConfirmado);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0 && Convert.ToInt16(paramConfirmado.Value) == 1)
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
        
        public bool EliminarTransaccion(int idTransaccion)
        {
            // Se debe usar solo cuando el detalle de la transaccion no se pudo crear
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spEliminarTransaccion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idTransaccion", idTransaccion);
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

        public List<TransaccionModel> ListarVentasSegunEmpleado(int idUsuario)
        {
            var lsTransaccion = new List<TransaccionModel>();
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spListarVentaPorEmpleado", cn);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cn.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var transaccion = new TransaccionModel
                    {
                        IdTransaccion = Convert.ToInt32(dr["IdTransaccion"]),
                        CodTransaccion = dr["CodTransaccion"]?.ToString() ?? "",
                        IdMetodoNavegacion = new MetodoPagoModel
                        {
                            Nombre = dr["metodo"]?.ToString() ?? "",
                        },
                        MontoBruto = Convert.ToDecimal(dr["MontoBruto"]),
                        Descuento = Convert.ToDecimal(dr["Descuento"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                        FechaTransaccion = Convert.ToDateTime(dr["FechaTransaccion"]),
                        EstadoTransaccion = dr["EstadoTransaccion"]?.ToString() ?? "",
                        IdCuentaNavegacion = new CuentaModel
                        {
                            Nombres = dr["usuario"]?.ToString() ?? "",
                        },
                        IdClienteNavegacion = new ClienteModel
                        {
                            Cliente = dr["cliente"]?.ToString() ?? "",
                        }
                    };
                    lsTransaccion.Add(transaccion);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return lsTransaccion;
        }

        public bool ConfirmarPagoPedido(string codTransaccion, int idPedido)
        {
            using var cn = Conexion.ObtenerConexion();
            using var cmd = new SqlCommand("spConfirmarPagoPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codTransaccion", codTransaccion);
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            try
            {
                SqlParameter confirmado = new("@confirmado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(confirmado);

                cn.Open();
                cmd.ExecuteNonQuery();
                if (Convert.ToInt16(confirmado.Value) == 1)
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

        #region Reportes
        #endregion Reportes

    }
}
