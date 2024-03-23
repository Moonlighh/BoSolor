using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logProveedorProducto
    {
        public static logProveedorProducto Instancia { get; } = new();
        private logProveedorProducto() { }

        #region CRU
        public bool CrearProveedorProductos(ProveedorProductoModel pro)
        {
            return datProveedorProducto.Instancia.CrearProveedorProducto(pro);
        }
        public List<ProveedorProductoModel> ListarProveedorProducto()
        {
            return datProveedorProducto.Instancia.ListarProveedorProductos();
        }
        #endregion CRU

        #region Otros
        public List<ProveedorProductoModel> ListarProductosCompra(int idProveedor)
        {
            return datProveedorProducto.Instancia.ListarProveedorProductoPorProveedor(idProveedor);
        }
        #endregion Otros
    }
}
