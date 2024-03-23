namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class RolModel
    {
        public int IdRol { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public bool Activo { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.MinValue;
    }
}
