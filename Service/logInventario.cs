using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logInventario
    {
        public static logInventario Instancia { get; } = new();
        private logInventario() { }

        #region CRUD
        public List<InventarioModel> ListarDetalleInventario()
        {
            return datInventario.Instancia.ListarDetalleInventario();
        }
        public bool CrearInventario(InventarioModel inventario, ref string mensaje)
        {
            try
            {
                int idGenerado = 1/*datInventario.Instancia.ListarInventario(inventario)*/;

                if (idGenerado == 0)
                {
                    throw new Exception("No se pudo crear el inventario");
                }
                return true;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                return false;
            }
        }
        public bool ActualizarInventario(InventarioModel inventario, ref string mensaje)
        {
            try
            {
                int idGenerado = 1/*datInventario.Instancia.ActualizarInventario(inventario)*/;

                if (idGenerado == 0)
                {
                    throw new Exception("No se pudo actualizar el inventario");
                }
                return true;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                return false;
            }
        }
        public bool EliminarInventario(int idInventario, ref string mensaje)
        {
            try
            {
                int idGenerado = 1/*datInventario.Instancia.EliminarInventario(idInventario)*/;

                if (idGenerado == 0)
                {
                    throw new Exception("No se pudo eliminar el inventario");
                }
                return true;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                return false;
            }
        }
        #endregion

        #region Otros
        public List<InventarioModel> MostrarProductosCompra(int idProveedor)
        {
            return datInventario.Instancia.MostrarProductosCompra(idProveedor);
        }
        // Permite mostrar los productos que se pueden vender al cliente seleccionado
        public List<InventarioModel> MostrarProductosVentaPedido()
        {
            return datInventario.Instancia.MostrarProductosVentaPedido();
        }
        public bool ValidarDisponibilidadProductoInventario(List<CarritoModel> lsCarritoVenta)
        {
            try
            {
                var listaDetalleInventario = datInventario.Instancia.ListarDetalleInventario();
                foreach (var item in lsCarritoVenta)
                {
                    var detalleInventario = listaDetalleInventario.First(i => i.IdProductoNavegacion.IdProducto == item.IdProducto);
                    if (item.CantidadPaquetes > detalleInventario.CantidadEnCajas)
                    {
                        throw new Exception($"No hay suficientes paquetes del producto solicite que se compren más {detalleInventario.IdProductoNavegacion.Nombre}");
                    }
                    if (item.CantidadUnidades > detalleInventario.CantidadEnUnidades)
                    {
                        throw new Exception($"No hay suficientes unidades del producto deberia abrir paquetes y volver a intentarlo {detalleInventario.IdProductoNavegacion.Nombre}");
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        public bool AbrirPaquete(int idProducto)
        {
            try
            {
                var listaDetalleInventario = datInventario.Instancia.ListarDetalleInventario();
                var inventarioEncontrado = listaDetalleInventario.First(i => i.IdProductoNavegacion.IdProducto == idProducto);
                if (inventarioEncontrado.CantidadEnCajas > 0)
                {
                    inventarioEncontrado.CantidadEnCajas--;
                    inventarioEncontrado.CantidadEnUnidades += inventarioEncontrado.IdProductoNavegacion.UnidadesPorConjunto;
                    return datInventario.Instancia.ActualizarInventario(inventarioEncontrado);
                }
                else
                {
                    throw new Exception($"No hay suficientes paquetes del producto {inventarioEncontrado.IdProductoNavegacion.Nombre}");
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion Otros
    }
}
