namespace TiendaVirtual.Models
{
    /// <summary>
    /// Representa un ítem dentro de un pedido confirmado.
    /// </summary>
    public class ItemPedido
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string TipoProducto { get; set; } = string.Empty;
        public string IconoTipo { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        /// <summary>
        /// Subtotal por ítem.
        /// </summary>
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}