using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class PedidoModel
    {
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo se permiten letras y números")]
        public string DireccionEntrega { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo se permiten letras y números")]
        public string Referencia { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo se permiten letras y números")]
        public string Detalle { get; set; } = null!;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaRecepcion { get; set; } = DateTime.Now;
        public DateTime FechaEntrega { get; set; } = DateTime.Now;
        public ClienteModel? IdClienteNavegacion { get; set; }
        public int IdCliente { get; set; }
        public TransaccionModel? IdTransaccionNavegacion { get; set; }
        public int IdTransaccion { get; set; }
        public EstadoPedidoModel? IdEstadoPedidoNavegacion { get; set; }
        public int IdEstadoPedido { get; set; }
        public CuentaModel? IdRepartidorNavegacion { get; set; }
        public int? IdRepartidor { get; set; }
    }
}
