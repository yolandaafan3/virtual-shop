using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    /// <summary>
    /// Representa a un cliente registrado en la tienda.
    /// Aplica encapsulación en datos sensibles y validaciones.
    /// </summary>
    public class Cliente
    {
        private string _email = string.Empty;
        private string _telefono = string.Empty;

        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar 50 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email
        {
            get => _email;
            set => _email = value?.ToLower().Trim() ?? string.Empty;
        }

        [Phone(ErrorMessage = "Ingrese un teléfono válido.")]
        public string Telefono
        {
            get => _telefono;
            set => _telefono = value?.Trim() ?? string.Empty;
        }

        public string Direccion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Nombre completo del cliente (propiedad calculada).
        /// </summary>
        public string NombreCompleto => $"{Nombre} {Apellido}";

        /// <summary>
        /// Cantidad de pedidos realizados.
        /// </summary>
        public int TotalPedidos { get; set; } = 0;
    }
}