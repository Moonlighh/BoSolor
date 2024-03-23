using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class ProductoModel
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string Nombre { get; set; } = null!;

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string? Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9 ]+$", ErrorMessage = "Solo se permiten letras y numeros.")]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioPorUnidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Precio Por Paquete")]
        public decimal PrecioPorConjunto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cantidad de unidades por paquete")]
        public int UnidadesPorConjunto { get; set; }
        public string? Imagen { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public CategoriaModel? IdCategoriaNavegacion { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria")]
        [Range(1, 100, ErrorMessage = "El categoria es obligatoria")]
        public int IdCategoria { get; set; }

    }
}
