using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class CategoriaModel
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La longitud debe estar entre 3 y 40 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        public string Descripcion { get; set; } = "";

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.MinValue;
    }
}
