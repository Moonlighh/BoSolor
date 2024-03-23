using BODEGA_SOLORZANO.Datos;
using BODEGA_SOLORZANO.Models.BoSolor;

namespace BODEGA_SOLORZANO.Service
{
    public class logUsuario
    {
        public static logUsuario Instancia { get; } = new();
        private logUsuario() { }

        #region Otros
        
        public UsuarioModel IniciarSesion(string userName, string contra)
        {
            UsuarioModel usuarioEncontrado = null;
            try
            {
                if (!(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(contra)))
                {
                    if (DateTime.Now.Hour > 24)
                    {
                        throw new Exception("No se puede ingresar despues de las 18:00 horas");
                    }
                    else
                    {
                        contra = Recursos.GetSHA256(contra);
                        usuarioEncontrado = datUsuario.Instancia.IniciarSesion(userName, contra);
                        if (usuarioEncontrado != null)
                        {
                            if (!usuarioEncontrado.Activo)
                            {
                                usuarioEncontrado = null;
                                throw new Exception("Usted ya no puede ingresar al sistema");
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return usuarioEncontrado;
        }
        #endregion Otros
    }
}
