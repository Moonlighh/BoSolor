using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Datos
{
    public class logRol
    {
        public static logRol Instancia { get; } = new();
        private logRol() { }

        #region CRUD

        public bool CrearRol(RolModel modelo)
        {
            return datRol.Instancia.CrearRol(modelo);
        }

        public List<RolModel> ListarRol()
        {
            return datRol.Instancia.ListarRoles();
        }
        #endregion CRUD

    }
}
