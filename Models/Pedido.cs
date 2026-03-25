namespace TiendaVirtual.Models
{
    /// <summary>
    /// Estados posibles de un pedido.
    /// </summary>
    public enum EstadoPedido
    {
        Pendiente,
        Procesando,
        Completado,
        Cancelado
    }

    /// <summary>
    /// Representa un pedido/compra completada en la tienda.
    /// </summary>
    public class Pedido
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
        public List<ItemPedido> Items { get; set; } = new();

        /// <summary>
        /// Total calculado sumando todos los ítems (Encapsulación).
        /// </summary>
        public decimal Total => Items.Sum(i => i.Subtotal);

        /// <summary>
        /// Cantidad total de productos en el pedido.
        /// </summary>
        public int CantidadTotal => Items.Sum(i => i.Cantidad);

        public string NotasAdicionales { get; set; } = string.Empty;
    }
}