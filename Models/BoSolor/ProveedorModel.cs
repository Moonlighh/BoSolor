namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class ProveedorModel
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; } = "";
        public string NumDocumento { get; set; } = "";
        public string Contacto { get; set; } = "";
        public bool Activo { get; set; }

    }
}
