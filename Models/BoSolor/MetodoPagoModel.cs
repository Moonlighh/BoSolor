using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class MetodoPagoModel
    {
        public int IdMetodo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string Descripcion { get; set; } = "";
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.MinValue;
    }
}
