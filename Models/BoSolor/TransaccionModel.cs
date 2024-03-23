using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class TransaccionModel
    {
        public int IdTransaccion { get; set; }
        public string CodTransaccion { get; set; } = "";
        public string TipoTransaccion { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "formato incorrecto para el sub total")]
        public decimal MontoBruto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números")]
        public decimal Descuento { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string EstadoTransaccion { get; set; } = "";
        public string Nota { get; set; } = "";
        public ClienteModel? IdClienteNavegacion { get; set; }
        public int IdCliente { get; set; }
        public ProveedorModel? IdProveedorNavegacion { get; set; }
        public int IdProveedor { get; set; }
        public MetodoPagoModel? IdMetodoNavegacion { get; set; }
        public int IdMetodo { get; set; }
        public CuentaModel? IdCuentaNavegacion { get; set; }
        public int IdCuenta { get; set; }
        public int IdPersona { get; set; }//Solo para registrar la compra en una unica transaccion
        public string Persona { get; set; } = "";

    }
}
