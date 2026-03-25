using TiendaVirtual.Models;

namespace TiendaVirtual.Services
{
    /// <summary>
    /// Servicio de gestión del carrito de compras (scoped por sesión).
    /// </summary>
    public class CarritoService
    {
        private readonly List<ItemCarrito> _items = new();

        /// <summary>
        /// Obtiene todos los ítems del carrito.
        /// </summary>
        public List<ItemCarrito> ObtenerItems() => _items.ToList();

        /// <summary>
        /// Agrega un producto al carrito o incrementa su cantidad.
        /// </summary>
        public (bool exito, string mensaje) AgregarItem(Producto producto, int cantidad = 1)
        {
            if (cantidad <= 0) return (false, "La cantidad debe ser mayor a 0.");
            if (producto.Stock < cantidad)
                return (false, $"Solo hay {producto.Stock} unidades disponibles.");

            var itemExistente = _items.FirstOrDefault(i => i.ProductoId == producto.Id);
            if (itemExistente != null)
            {
                int nuevaCantidad = itemExistente.Cantidad + cantidad;
                if (nuevaCantidad > producto.Stock)
                    return (false, $"No puedes agregar más. Stock disponible: {producto.Stock}.");
                itemExistente.Cantidad = nuevaCantidad;
            }
            else
            {
                _items.Add(new ItemCarrito
                {
                    ProductoId = producto.Id,
                    NombreProducto = producto.Nombre,
                    CodigoProducto = producto.Codigo,
                    TipoProducto = producto.TipoProducto,
                    IconoTipo = producto.IconoTipo,
                    PrecioUnitario = producto.Precio,
                    Cantidad = cantidad
                });
            }

            return (true, $"'{producto.Nombre}' agregado al carrito.");
        }

        /// <summary>
        /// Actualiza la cantidad de un ítem en el carrito.
        /// </summary>
        public bool ActualizarCantidad(int productoId, int nuevaCantidad)
        {
            var item = _items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null) return false;
            if (nuevaCantidad <= 0)
            {
                _items.Remove(item);
                return true;
            }
            item.Cantidad = nuevaCantidad;
            return true;
        }

        /// <summary>
        /// Elimina un ítem del carrito.
        /// </summary>
        public bool EliminarItem(int productoId)
        {
            var item = _items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null) return false;
            _items.Remove(item);
            return true;
        }

        /// <summary>
        /// Limpia todo el carrito.
        /// </summary>
        public void VaciarCarrito() => _items.Clear();

        /// <summary>
        /// Total del carrito (Encapsulación).
        /// </summary>
        public decimal ObtenerTotal() => _items.Sum(i => i.Subtotal);

        /// <summary>
        /// Cantidad total de ítems (para el badge del carrito).
        /// </summary>
        public int ObtenerCantidadTotal() => _items.Sum(i => i.Cantidad);

        /// <summary>
        /// Verifica si el carrito está vacío.
        /// </summary>
        public bool EstaVacio() => !_items.Any();
    }
}