namespace TiendaVirtual.Models
{
    /// <summary>
    /// Representa un ítem dentro del carrito de compras.
    /// </summary>
    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoProducto { get; set; } = string.Empty;
        public string TipoProducto { get; set; } = string.Empty;
        public string IconoTipo { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        /// <summary>
        /// Subtotal calculado (Encapsulación de la lógica de cálculo).
        /// </summary>
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}