using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class ClienteModel
    {
        public int IdCliente { get; set; }
        public string? Cliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números")]
        public string NumDocumento { get; set; } = "";
        public DateTime FechaRegistro { get; set; } = DateTime.MinValue;

        // Propiedades adicionales para el reporte de clientes
        public int CantidadTransacciones { get; set; }

    }
}
