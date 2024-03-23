namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; } = "";
        public string Usuario { get; set; } = "";
        public bool Activo { get; set; }
    }
}

