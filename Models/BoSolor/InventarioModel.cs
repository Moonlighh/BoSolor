namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class InventarioModel
    {
        public int IdInventario { get; set; }
        public ProductoModel IdProductoNavegacion { get; set; } = null!;
        public int IdProducto { get; set; }
        public int CantidadEnCajas { get; set; }
        public int CantidadEnUnidades { get; set; }


        // Estos atributos unicamente se llenan cuando se van a listar los productos para una compra.
        // Este atributo no existe en la base de datos pero se usa solo para evitar consultar a la tabla proveedorProducto
        // y obtener el precio de compra de un producto.
        public decimal PrecioCompra { get; set; }
        // Este atributo no existe en la tabla de la base de datos pero se usa solo para evitar consultar a la tabla proveedorProducto
        public int IdProveedor { get; set; }

    }

}
