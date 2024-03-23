using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logMetodoPago
    {
        public static logMetodoPago Instancia { get; } = new();
        private logMetodoPago() { }

        #region CRUD
        public bool CrearMetodoPago(MetodoPagoModel metodoPago)
        {
            return datMetodoPago.Instancia.CrearMetodoPago(metodoPago);
        }

        public List<MetodoPagoModel> ListarMetodosPago()
        {
            return datMetodoPago.Instancia.ListarMetodosPago();
        }

        public bool ActualizarMetodoPago(MetodoPagoModel metodoPago)
        {
            return datMetodoPago.Instancia.ActualizarMetodoPago(metodoPago);
        }

        public bool EliminarMetodoPago(int id)
        {
            try
            {
                return datMetodoPago.Instancia.EliminarMetodoPago(id);
            }
            catch (Exception e)
            {
                throw new Exception("El método de pago no se puede eliminar", e);
            }
        }
        #endregion CRUD

    }
}
