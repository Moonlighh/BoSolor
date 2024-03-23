using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logPedido
    {
        public static logPedido Instancia { get; } = new();
        private logPedido() { }

        #region CRUD
        public bool CrearPedido(PedidoModel pe)
        {
            return datPedido.Instancia.CrearPedido(pe);
        }

        public List<PedidoModel> ListarPedidos()
        {
            return datPedido.Instancia.ListarPedidos();
        }
        #endregion CRUD

        #region Otros métodos
        public List<PedidoModel> ListarPedidosPendientes()
        {
            return datPedido.Instancia.ListarPedidosPendientes();
        }
        public bool TomarPedido(int idPedido, int idRepartidor)
        {
            return datPedido.Instancia.TomarPedido(idPedido, idRepartidor);
        }
        public bool ConfirmarEntrega(int idPedido)
        {
            // El procedimiento almacenado valida que no se vuelva actualizar la fecha de entrega si es que esta ya fue actualizada
            return datPedido.Instancia.ConfirmarEntrega(idPedido);
        }
        public List<PedidoModel> ListarPedidosPorRepartidor(int idRepartidor)
        {
            return datPedido.Instancia.ListarPedidosRepartidor(idRepartidor);
        }
        #endregion Otros métodos

    }
}
