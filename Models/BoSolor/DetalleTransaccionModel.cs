using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class DetalleTransaccionModel
    {
        public int IdDetalleTransaccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números")]
        public int CantidadPorConjunto { get; set; }
        public int CantidadUnidadades { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "formato incorrecto para el sub total")]
        public decimal SubTotal { get; set; }
        public TransaccionModel? IdTransaccionNavegacion { get; set; }
        public int IdTransaccion { get; set; }
        public ProductoModel? IdProductoNavegacion { get; set; }
        public int IdProducto { get; set; }

    }
}
