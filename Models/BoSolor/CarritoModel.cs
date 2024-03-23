using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class CarritoModel
    {
        [Key]
        public int IdCarrito { get; set; }

        [Required]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        public int IdProveedor { get; set; }

        public int IdCliente { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "El nombre del producto no puede exceder los 40 caracteres.")]
        public string Producto { get; set; } = null!;

        [Required]
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int CantidadPaquetes { get; set; }
        public int CantidadUnidades { get; set; }

        [Required]
        public decimal PrecioPaquete { get; set; } // Tanto para compra como venta-Es el precio por paquete
        [Required]
        public decimal PrecioPorUnidad { get; set; }
        [Required]
        public int UnidadesPorConjunto { get; set; }
        public decimal Subtotal { get; set; }
        public int IdMetodoPago { get; set; }

    }
}
