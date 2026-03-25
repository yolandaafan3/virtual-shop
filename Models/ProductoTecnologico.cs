using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    /// <summary>
    /// Clase hija que representa un producto tecnológico.
    /// Hereda de Producto y agrega atributos propios de tecnología.
    /// </summary>
    public class ProductoTecnologico : Producto
    {
        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "La garantía debe estar entre 1 y 10 años.")]
        public int GarantiaAnios { get; set; }

        public string Especificaciones { get; set; } = string.Empty;

        // --- Polimorfismo: override de propiedades y métodos abstractos ---

        public override string TipoProducto => "Tecnología";
        public override string IconoTipo => "💻";
        public override string ColorTipo => "tech";

        public override string ObtenerDescripcionDetallada()
        {
            return $"Marca: {Marca} | Modelo: {Modelo} | Garantía: {GarantiaAnios} año(s) | {Especificaciones}";
        }

        public override string MostrarInfo()
        {
            return base.MostrarInfo() + $" | Marca: {Marca} | Garantía: {GarantiaAnios} año(s)";
        }
    }
}