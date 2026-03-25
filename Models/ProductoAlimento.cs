using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    /// <summary>
    /// Clase hija que representa un producto alimenticio.
    /// Hereda de Producto y agrega fecha de vencimiento y peso.
    /// </summary>
    public class ProductoAlimento : Producto
    {
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateTime FechaVencimiento { get; set; } = DateTime.Now.AddMonths(6);

        [Required(ErrorMessage = "El peso/volumen es obligatorio.")]
        public string PesoVolumen { get; set; } = string.Empty;

        public bool EsOrganico { get; set; }
        public bool RequiereRefrigeracion { get; set; }
        public string Ingredientes { get; set; } = string.Empty;

        // --- Polimorfismo ---

        public override string TipoProducto => "Alimento";
        public override string IconoTipo => "🛒";
        public override string ColorTipo => "alimento";

        /// <summary>
        /// Verifica si el producto está por vencer (en los próximos 30 días).
        /// </summary>
        public bool EstaPorVencer() => FechaVencimiento <= DateTime.Now.AddDays(30);

        /// <summary>
        /// Verifica si el producto ya venció.
        /// </summary>
        public bool EstaVencido() => FechaVencimiento < DateTime.Now;

        public override string ObtenerDescripcionDetallada()
        {
            var estado = EstaVencido() ? "⚠️ VENCIDO" : EstaPorVencer() ? "⚠️ Por vencer" : "✅ Vigente";
            return $"Vence: {FechaVencimiento:dd/MM/yyyy} ({estado}) | Peso/Vol: {PesoVolumen} | Orgánico: {(EsOrganico ? "Sí" : "No")} | Refrigeración: {(RequiereRefrigeracion ? "Sí" : "No")}";
        }

        public override string MostrarInfo()
        {
            return base.MostrarInfo() + $" | Vence: {FechaVencimiento:dd/MM/yyyy}";
        }
    }
}