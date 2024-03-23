namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class RolMenuModel
    {
        public int IdRolMenu { get; set; }
        public RolModel IdRolNavegacion { get; set; } = new();
        public int IdRol { get; set; }
        public MenuModel IdMenuNavegacion { get; set; } = new();
        public int IdMenu { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.MinValue;
    }
}
