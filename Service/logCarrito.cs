using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logCarrito
    {
        public static logCarrito Instancia { get; } = new();
        private logCarrito() { }

        private List<CarritoModel> lsCarrito = new();//Funciona para compra y venta
        private List<CarritoModel> lsCarritoPedido = new();//Funciona para pedidos
        private int idCarritoCompra = 0;
        private int idCarritoVenta = 0;
        private int idCarritoPedido = 0;

        #region Compra y Venta
        public bool AgregarCarritoCompra(CarritoModel carrito)
        {
            try
            {
                carrito.IdCarrito = ++idCarritoCompra;
                foreach (var item in lsCarrito)
                {
                    if (item.IdProveedor != carrito.IdProveedor)
                    {
                        throw new Exception("No se puede agregar productos de distintos proveedores");
                    }
                    if (item.IdProducto == carrito.IdProducto)
                    {
                        throw new Exception("El producto ya está agregado");
                    }
                }
                carrito.Subtotal = carrito.CantidadPaquetes * carrito.PrecioPaquete;
                lsCarrito.Add(carrito);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public bool AgregarCarritoVenta(int idProducto, int cantidadPaquetes, int cantidadUnidades, int idUsuario)
        {
            // Este metodo permite agregar paquetes y unidades
            try
            {
                if (cantidadPaquetes <= 0 && cantidadUnidades <= 0)
                {
                    throw new Exception("La cantidad de unidades y paquetes no puede ser menor o igual a 0");
                }

                // Le agrego first pero no lo creo necesario ya que el idProducto es unico
                var detalleInventario = logInventario.Instancia.ListarDetalleInventario().Where(i => i.IdProductoNavegacion.IdProducto == idProducto).First();

                // Vamos aplicar la logicas de las unidades y paquetes
                if (cantidadPaquetes > 0)
                {
                    if (cantidadPaquetes > detalleInventario.CantidadEnCajas)
                    {
                        throw new Exception("La cantidad de paquetes no puede ser mayor a la cantidad de paquetes en inventario");
                    }
                }
                if (cantidadUnidades > 0)
                {
                    if (cantidadUnidades > detalleInventario.CantidadEnUnidades)
                    {
                        throw new Exception("La cantidad de unidades no puede ser mayor a la cantidad de unidades en inventario. Deberia abrir paquetes antes de intentar agregar");
                    }
                }

                CarritoModel carrito = new()
                {
                    IdProducto = detalleInventario.IdProductoNavegacion.IdProducto,
                    Producto = detalleInventario.IdProductoNavegacion.Nombre,
                    CantidadPaquetes = cantidadPaquetes,
                    CantidadUnidades = cantidadUnidades,
                    PrecioPaquete = detalleInventario.IdProductoNavegacion.PrecioPorConjunto,//Precio por paquete
                    PrecioPorUnidad = detalleInventario.IdProductoNavegacion.PrecioPorUnidad,
                    IdUsuario = idUsuario,
                    IdCarrito = ++idCarritoVenta
                };

                foreach (var item in lsCarrito)
                {
                    if (item.IdProducto == carrito.IdProducto)
                    {
                        throw new Exception("El producto ya existe en el carrito");
                    }
                }
                carrito.Subtotal = carrito.CantidadPaquetes * carrito.PrecioPaquete;
                carrito.Subtotal += carrito.CantidadUnidades * carrito.PrecioPorUnidad;
                lsCarrito.Add(carrito);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public List<CarritoModel> ListarCarritoCompraVenta(int idUsuario)
        {
            return lsCarrito.Where(c => c.IdUsuario == idUsuario).ToList();
        }
        public bool EliminarCarritoCompraVenta(int id)
        {
            // Este metodo elimina un producto del carrito
            try
            {
                if (lsCarrito != null)
                {
                    var itemToRemove = lsCarrito.FirstOrDefault(c => c.IdCarrito == id);
                    if (itemToRemove != null)
                    {
                        lsCarrito.Remove(itemToRemove);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EliminarProductosPorUsuario(int idUsuario)
        {
            // Este metodo elimina todos los productos del usuario despues de una compra o venta
            try
            {
                if (lsCarrito != null)
                {
                    var itemToRemove = lsCarrito.Where(c => c.IdUsuario == idUsuario).ToList();
                    if (itemToRemove != null)
                    {
                        foreach (var item in itemToRemove)
                        {
                            lsCarrito.Remove(item);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion Compra y Venta

        #region Pedido
        public bool AgregarCarritoPedido(int idProducto, int cantidadPaquetes, int idUsuario)
        {
            // Este metodo solo permite agregar paquetes
            try
            {
                if (cantidadPaquetes <= 0)
                {
                    throw new Exception("La cantidad paquetes no puede ser menor o igual a 0");
                }

                // Le agrego first pero no lo creo necesario ya que el idProducto es unico
                var detalleInventario = logInventario.Instancia.ListarDetalleInventario().Where(i => i.IdProductoNavegacion.IdProducto == idProducto).First();

                if (cantidadPaquetes > detalleInventario.CantidadEnCajas)
                {
                    throw new Exception("La cantidad de paquetes no puede ser mayor a la cantidad de paquetes en inventario");
                }

                CarritoModel carrito = new()
                {
                    IdProducto = detalleInventario.IdProductoNavegacion.IdProducto,
                    Producto = detalleInventario.IdProductoNavegacion.Nombre,
                    CantidadPaquetes = cantidadPaquetes,
                    PrecioPaquete = detalleInventario.IdProductoNavegacion.PrecioPorConjunto,
                    IdUsuario = idUsuario,
                    IdCarrito = ++idCarritoPedido
                };

                foreach (var item in lsCarritoPedido)
                {
                    if (item.IdProducto == carrito.IdProducto)
                    {
                        throw new Exception("El producto ya existe en el carrito");
                    }
                }
                carrito.Subtotal = carrito.CantidadPaquetes * carrito.PrecioPaquete;
                carrito.Subtotal += carrito.CantidadUnidades * carrito.PrecioPorUnidad;
                lsCarritoPedido.Add(carrito);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public List<CarritoModel> ListarCarritoPedido(int idUsuario)
        {
            return lsCarritoPedido.Where(c => c.IdUsuario == idUsuario).ToList();
        }
        public bool EliminarCarritoPedido(int id)
        {
            try
            {
                if (lsCarritoPedido != null)
                {
                    var itemToRemove = lsCarritoPedido.FirstOrDefault(c => c.IdCarrito == id);
                    if (itemToRemove != null)
                    {
                        lsCarritoPedido.Remove(itemToRemove);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EliminarProductosPorUsuarioPedido(int idUsuario)
        {
            try
            {
                if (lsCarritoPedido != null)
                {
                    var itemToRemove = lsCarritoPedido.Where(c => c.IdUsuario == idUsuario).ToList();
                    if (itemToRemove != null)
                    {
                        foreach (var item in itemToRemove)
                        {
                            lsCarritoPedido.Remove(item);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion Pedido
    }
}
