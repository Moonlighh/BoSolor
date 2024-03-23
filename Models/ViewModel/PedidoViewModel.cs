namespace BODEGA_SOLORZANO.Models.ViewModel
{
    public class PedidoViewModel
    {
        public string Dni { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string DetallePedido { get; set; }
        public decimal Descuento { get; set; }
        public string DetalleTransaccion { get; set; }
        public int IdMetodoPago { get; set; }
    }
}
