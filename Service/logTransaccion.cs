using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Models.ViewModel;

namespace BODEGA_SOLORZANO.Service
{
    public enum TipoTransaccion
    {
        Todas,
        Compra,
        Venta,
        Pedido,
        VentasPorEmpleado
    }

    public class logTransaccion
    {
        public static logTransaccion Instancia { get; } = new();
        private logTransaccion() { }

        #region CR
        public bool CrearTransaccionCompra(List<CarritoModel> lsCarrito, ref string mensaje)
        {
            try
            {
                var codigo = "C-" + Guid.NewGuid().ToString();
                var transaccion = new TransaccionModel()
                {
                    CodTransaccion = codigo,
                    TipoTransaccion = "COMPRA",
                    MontoBruto = 0,
                    Descuento = 0,
                    MontoTotal = lsCarrito.Sum(c => c.Subtotal),
                    EstadoTransaccion = "PAGADO",
                    Nota = "La transacción se pago previo al registro",
                    IdPersona = lsCarrito.First().IdProveedor,
                    IdMetodo = 1,
                    IdCuenta = lsCarrito.First().IdUsuario,
                };

                int idGenerado = datTransaccion.Instancia.IniciarTransaccion(transaccion);

                if (idGenerado == 0)
                {
                    throw new Exception("No se pudo crear la transacción");
                }

                foreach (var item in lsCarrito)
                {
                    var detalle = new DetalleTransaccionModel()
                    {
                        CantidadPorConjunto = item.CantidadPaquetes,//En este caso solo seran conjuntos
                        CantidadUnidadades = 0,
                        SubTotal = item.Subtotal,
                        IdTransaccion = idGenerado,
                        IdProducto = item.IdProducto,
                    };
                    bool exito = datTransaccion.Instancia.CrearDetalleTransaccion(detalle);
                    if (!exito)
                    {
                        throw new Exception($"Error al crear el detalle del producto con id: {item.IdProducto}");
                    }
                }
                mensaje = $"La transacción {codigo} se realizó con exito";
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> CrearTransaccionVenta(List<CarritoModel> lsCarrito, object objetoAnonimo)
        {
            try
            {
                dynamic dynObjetoAnonimo = objetoAnonimo;

                string objAnonimoValido = ValidarObjetoAnonimoVenta(dynObjetoAnonimo);

                if (!string.IsNullOrEmpty(objAnonimoValido))//Verifica si el objeto anonimo es valido-Devuelve mensajes de error para cada campo
                {
                    throw new Exception(objAnonimoValido);
                }

                ClienteModel cliente = await ObtenerCliente(dynObjetoAnonimo.Dni);
                ValidarCliente(cliente);

                if (dynObjetoAnonimo.Descuento >= (lsCarrito.Sum(c => c.Subtotal) / 3))//Se define esta regla de negocio para evitar perdidas
                {
                    throw new Exception("El descuento no puede superar la tercera parte de la compra total");
                }

                var codigo = GenerarCodigoTransaccion("V");
                var transaccion = new TransaccionModel()
                {
                    CodTransaccion = codigo,
                    TipoTransaccion = "VENTA",
                    MontoBruto = lsCarrito.Sum(c => c.Subtotal),
                    Descuento = dynObjetoAnonimo.Descuento,
                    MontoTotal = lsCarrito.Sum(c => c.Subtotal) - dynObjetoAnonimo.Descuento,
                    EstadoTransaccion = "PAGADO",
                    Nota = dynObjetoAnonimo.Nota,
                    IdPersona = cliente.IdCliente,
                    IdMetodo = dynObjetoAnonimo.IdMetodoPago,
                    IdCuenta = lsCarrito.First().IdUsuario,
                };

                // Antes de iniciar la transaccion verificamos disponibilidad de productos en el inventario

                logInventario.Instancia.ValidarDisponibilidadProductoInventario(lsCarrito);//Este metodo asegura que se lanzen excepciones si no hay suficientes productos

                int idGenerado = datTransaccion.Instancia.IniciarTransaccion(transaccion);

                if (idGenerado == 0)
                {
                    throw new Exception("No se pudo crear la venta, intente de nuevo");
                }

                foreach (var item in lsCarrito)
                {
                    var detalle = new DetalleTransaccionModel()
                    {
                        CantidadPorConjunto = item.CantidadPaquetes,//En este caso solo seran conjuntos
                        CantidadUnidadades = item.CantidadUnidades,
                        SubTotal = item.Subtotal,
                        IdTransaccion = idGenerado,
                        IdProducto = item.IdProducto,
                    };
                    bool exito = datTransaccion.Instancia.CrearDetalleTransaccion(detalle);
                    if (!exito)
                    {
                        datTransaccion.Instancia.EliminarTransaccion(idGenerado);
                        throw new Exception($"Error al crear el detalle del producto con id: {item.IdProducto} y nombre {item.Producto}");
                    }
                }
                return $"Se registró una transacción con el codigo {codigo} hacia el cliente {cliente.Cliente}";

            }
            catch
            {
                throw;
            }
        }

        public async Task<string> CrearTransaccionPedido(List<CarritoModel> lsCarrito, PedidoViewModel modelo)
        {
            try
            {
                ClienteModel cliente = await ObtenerCliente(modelo.Dni);
                ValidarCliente(cliente);
                ValidarDescuento(modelo.Descuento, lsCarrito);
                var codigo = GenerarCodigoTransaccion("P");
                var transaccion = CrearTransaccionModel(lsCarrito, modelo, cliente, codigo);
                // Antes de iniciar la transaccion verificamos disponibilidad de productos en el inventario
                logInventario.Instancia.ValidarDisponibilidadProductoInventario(lsCarrito);//Asegura que se lanzen excepciones si no hay suficientes productos
                int idTransaccionGenerado = IniciarTransaccion(transaccion);
                CrearDetallesTransaccion(lsCarrito, idTransaccionGenerado);

                var pedido = new PedidoModel()
                {
                    DireccionEntrega = modelo.Direccion,
                    Referencia = modelo.Referencia,
                    Detalle = modelo.DetallePedido,
                    IdCliente = cliente.IdCliente,
                    IdTransaccion = idTransaccionGenerado,
                    IdEstadoPedido = 1,//Pendiente
                };

                bool pedidoCreado = datPedido.Instancia.CrearPedido(pedido);
                if (!pedidoCreado)
                {
                    throw new Exception("Error al crear el pedido.");
                }

                return $"Se registró una transacción con el código {codigo} hacia el cliente {cliente.Cliente}. Recuerda hacer seguimiento al pedido";
            }
            catch (Exception ex)
            {
                // Considera agregar un registro de este error
                throw new Exception($"Error al crear la transacción del pedido: {ex.Message}", ex);
            }
        }

        public List<TransaccionModel> ListarTransacciones(TipoTransaccion tipo, string idUsuario = "0")
        {
            int idUsuarioInt = Convert.ToInt32(idUsuario);
            switch (tipo)
            {
                case TipoTransaccion.Todas:
                    return datTransaccion.Instancia.ListarTransacciones();
                case TipoTransaccion.Compra:
                    return datTransaccion.Instancia.ListarTransaccionCompra();
                case TipoTransaccion.Venta:
                    return datTransaccion.Instancia.ListarTransaccionVenta();
                case TipoTransaccion.Pedido:
                    return datTransaccion.Instancia.ListarTransaccionPedido();
                case TipoTransaccion.VentasPorEmpleado:
                    if (idUsuarioInt == 0)
                    {
                        throw new ArgumentException("Se requiere un ID de usuario para listar ventas por empleado.");
                    }
                    return datTransaccion.Instancia.ListarVentasSegunEmpleado(idUsuarioInt);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipo), tipo, "Tipo de transacción no reconocido.");
            }
        }
        #endregion CR

        #region Otros
        public bool ConfirmarPagoPedido(string codigoTransaccion, int idPedido)
        {
            if (!string.IsNullOrEmpty(codigoTransaccion) && idPedido > 0)
            {
                return datTransaccion.Instancia.ConfirmarPagoPedido(codigoTransaccion, idPedido);
            }
            return false;
        }
        #endregion Otros

        #region Detalle
        public List<DetalleTransaccionModel> ListarDetalleTransaccion(int idTransaccion)
        {
            return datTransaccion.Instancia.ListarDetalleTransaccion(idTransaccion);

        }
        public List<DetalleTransaccionModel> ListarDetalleTransaccionPedido(int idPedido)
        {
            return datTransaccion.Instancia.ListarDetalleTransaccionPedido(idPedido);
        }
        #endregion Detalle

        #region Metodos privados
        private static async Task<ClienteModel> ObtenerCliente(string dni)
        {
            try
            {
                var clienteExiste = logCliente.Instancia.ListarClientes().Where(c => c.NumDocumento == dni);
                if (clienteExiste.Any())// Determina si la secuencia contiene elementos
                {
                    return clienteExiste.First();
                }
                else
                {
                    var clienteNuevo = new ClienteModel()
                    {
                        NumDocumento = dni,
                    };

                    bool exito = await logCliente.Instancia.CrearCliente(clienteNuevo);
                    if (exito)
                    {
                        var clienteCreado = logCliente.Instancia.ListarClientes().Where(c => c.NumDocumento == dni).First();
                        return clienteCreado;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        private static string ValidarObjetoAnonimoVenta(dynamic objAnonimoVenta)
        {
            try
            {
                if (Convert.ToInt16(objAnonimoVenta.Dni.Length) != 8)
                {
                    return "El DNI debe tener 8 digitos";
                }
                if (Convert.ToInt16(objAnonimoVenta.IdMetodoPago) <= 0)
                {
                    return "Debe seleccionar un metodo de pago";
                }
            }
            catch
            {
                return "Intente de nuevo";
            }
            return string.Empty;
        }
        private static void ValidarCliente(ClienteModel cliente)
        {
            if (cliente == null)
            {
                throw new Exception("No se pudo obtener el cliente");
            }
        }
        private static void ValidarDescuento(decimal descuento, List<CarritoModel> lsCarrito)
        {
            if (descuento >= (lsCarrito.Sum(c => c.Subtotal) / 3))//Se define esta regla de negocio para evitar perdidas
            {
                throw new Exception("El descuento no puede superar la tercera del pedido total");
            }
        }
        private static string GenerarCodigoTransaccion(string tipoTransaccion)
        {
            return $"{tipoTransaccion}-{Guid.NewGuid()}";
        }
        private TransaccionModel CrearTransaccionModel(List<CarritoModel> lsCarrito, PedidoViewModel modelo, ClienteModel cliente, string codigo)
        {
            var montoBruto = CalcularMontoBruto(lsCarrito);
            var montoTotal = CalcularMontoTotal(montoBruto, modelo.Descuento);

            var transaccion = new TransaccionModel()
            {
                CodTransaccion = codigo,
                TipoTransaccion = "PEDIDO",
                MontoBruto = montoBruto,
                Descuento = modelo.Descuento,
                MontoTotal = montoTotal,
                EstadoTransaccion = "EN ESPERA",
                Nota = modelo.DetalleTransaccion,
                IdPersona = cliente.IdCliente,
                IdMetodo = modelo.IdMetodoPago,
                IdCuenta = lsCarrito.First().IdUsuario,
            };

            return transaccion;
        }
        private static decimal CalcularMontoBruto(List<CarritoModel> lsCarrito)
        {
            return lsCarrito.Sum(c => c.Subtotal);
        }
        private static decimal CalcularMontoTotal(decimal montoBruto, decimal descuento)
        {
            return montoBruto - descuento;
        }
        private static int IniciarTransaccion(TransaccionModel transaccion)
        {
            int idGenerado = datTransaccion.Instancia.IniciarTransaccion(transaccion);

            if (idGenerado == 0)
            {
                throw new Exception("No se pudo crear la transaccion, intente de nuevo");
            }
            return idGenerado;

        }
        private static bool CrearDetallesTransaccion(List<CarritoModel> lsCarrito, int idTransaccion)
        {
            if (!lsCarrito.Any())
            {
                throw new Exception("La lista del carrito está vacía.");
            }

            foreach (var item in lsCarrito)
            {
                var detalle = new DetalleTransaccionModel()
                {
                    CantidadPorConjunto = item.CantidadPaquetes,
                    CantidadUnidadades = item.CantidadUnidades,
                    SubTotal = item.Subtotal,
                    IdTransaccion = idTransaccion,
                    IdProducto = item.IdProducto,
                };

                bool exito = datTransaccion.Instancia.CrearDetalleTransaccion(detalle);
                if (!exito)
                {
                    // Considerar agregar un registro de error
                    EliminarDetallesYTransaccion(idTransaccion);
                    return false;
                }
            }

            return true;
        }
        private static void EliminarDetallesYTransaccion(int idTransaccion)
        {
            bool detallesEliminados = datTransaccion.Instancia.EliminarDetalleTransaccion(idTransaccion);
            bool transaccionEliminada = datTransaccion.Instancia.EliminarTransaccion(idTransaccion);

            if (!detallesEliminados || !transaccionEliminada)
            {
                throw new Exception($"Error al eliminar detalles o transacción: Transacción -> {idTransaccion}");
            }
        }
        #endregion Metodos privados
    }
}
