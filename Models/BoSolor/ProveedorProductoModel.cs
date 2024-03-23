namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class ProveedorProductoModel
    {
        public int IdProveedorProducto { get; set; }
        public ProveedorModel? IdProveedorNavegacion { get; set; }
        public int IdProveedor { get; set; }
        public ProductoModel? IdProductoNavegacion { get; set; }
        public int IdProducto { get; set; }
        public decimal PrecioCompra { get; set; }
    }

}
