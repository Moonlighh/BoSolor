using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logProducto
    {
        public static logProducto Instancia { get; } = new();
        private logProducto() { }

        #region CRUD
        public bool CrearProducto(ProductoModel prod)
        {
            return datProducto.Instancia.CrearProducto(prod);
        }
        public List<ProductoModel> ListarProductos()
        {
            return datProducto.Instancia.ListarProductos();

        }
        public bool ActualizarProducto(ProductoModel prod)
        {
            return datProducto.Instancia.ActualizarProducto(prod);
        }
        public bool EliminarProducto(int id)
        {
            try
            {
                return datProducto.Instancia.EliminarProducto(id);
            }
            catch (Exception e)
            {
                throw new Exception("El producto no se puede eliminar", e);
            }
        }
        #endregion CRUD

    }
}
