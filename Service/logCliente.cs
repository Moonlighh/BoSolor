using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;
namespace BODEGA_SOLORZANO.Service
{

    public class logCliente
    {
        public static logCliente Instancia { get; } = new();
        private logCliente() { }

        #region CRUD

        public async Task<bool> CrearCliente(ClienteModel cliente)
        {
            try
            {
                ApiService apiService = new();
                cliente.Cliente = await apiService.ConsultarAPIsPeru(cliente.NumDocumento);
                return datCliente.Instancia.CrearCliente(cliente);
            }
            catch
            {
                return false;
            }
        }

        public List<ClienteModel> ListarClientes()
        {
            return datCliente.Instancia.ListarClientes();
        }

        public bool ActualizarCliente(ClienteModel cliente)
        {
            return datCliente.Instancia.ActualizarCliente(cliente);
        }

        public bool EliminarCliente(int id)
        {
            return datCliente.Instancia.EliminarCliente(id);
        }
        #endregion CRUD

    }

}
