namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class MenuModel
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Controlador { get; set; } = "";
        public string PaginaAccion { get; set; } = "";
        public bool EsActivo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.MinValue;
        public int IdMenuPadre { get; set; }
        public MenuModel? IdMenuPadreNavegacion { get; set; }
        public List<MenuModel>? ListaMenu { get; set; }
    }
}
