using System.ComponentModel.DataAnnotations;

namespace BODEGA_SOLORZANO.Models.BoSolor
{
    public class CuentaModel
    {
        public int IdCuenta { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public string DNI { get; set; } = "";

        public string Nombres { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El número de teléfono debe tener 9 dígitos.")]
        public string Telefono { get; set; } = "";

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [RegularExpression(@"^[a-zA-ZÑñ]{5,25}$", ErrorMessage = "El nombre de usuario debe tener entre 5 y 25 caracteres y solo puede contener letras")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Constraseña requerida")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "La contraseña debe tener entre 8 y 15 caracteres, al menos un dígito, al menos una minúscula, al menos una mayúscula y al menos un caracter especial")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirme su contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]//Solo para la vista
        public string ConfirmPassword { get; set; } = "";

        public bool Activo { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.MinValue;
        public DateTime FechaFin { get; set; } = DateTime.MinValue;

        public RolModel? IdRolNavegacion { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Range(1, 100, ErrorMessage = "El rol es obligatorio")]
        public int IdRol { get; set; }
    }
}
