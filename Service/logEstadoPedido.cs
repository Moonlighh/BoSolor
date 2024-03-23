using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logEstadoPedido
    {
        public static logEstadoPedido Instancia { get; } = new();
        private logEstadoPedido() { }

        #region R
        // Listar
        public List<EstadoPedidoModel> ListarEstadoPedido()
        {
            return datEstadoPedido.Instancia.ListarEstadoPedidos();
        }
        #endregion R

    }
}
