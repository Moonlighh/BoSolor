using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class EstadoPedidoModel
    {
        private int idEstadoPedido;
        private string nombreEstado = null!;
        private string descripcion = null!;
        private DateTime fechaCreacion;

        #region Get and Set

        public int IdEstadoPedido
        {
            get { return idEstadoPedido; }
            set { idEstadoPedido = value; }
        }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        public string NombreEstado
        {
            get { return nombreEstado; }
            set { nombreEstado = value; }
        }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La longitud debe estar entre 5 y 100 caracteres")]
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }
        #endregion
    }
}
