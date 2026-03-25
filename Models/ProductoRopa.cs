using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    /// <summary>
    /// Clase hija que representa un producto de ropa/moda.
    /// Hereda de Producto y agrega talla, color y material.
    /// </summary>
    public class ProductoRopa : Producto
    {
        [Required(ErrorMessage = "La talla es obligatoria.")]
        public string Talla { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio.")]
        public string Color { get; set; } = string.Empty;

        public string Material { get; set; } = string.Empty;

        public string Genero { get; set; } = "Unisex";

        // --- Polimorfismo ---

        public override string TipoProducto => "Ropa";
        public override string IconoTipo => "👕";
        public override string ColorTipo => "ropa";

        public override string ObtenerDescripcionDetallada()
        {
            return $"Talla: {Talla} | Color: {Color} | Material: {Material} | Género: {Genero}";
        }

        public override string MostrarInfo()
        {
            return base.MostrarInfo() + $" | Talla: {Talla} | Color: {Color}";
        }
    }
}