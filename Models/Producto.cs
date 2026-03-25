using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    /// <summary>
    /// Clase abstracta base que representa cualquier producto en la tienda.
    /// Define la estructura común de todos los productos (Abstracción + Herencia).
    /// </summary>
    public abstract class Producto
    {
        // --- Encapsulación: atributos privados con propiedades públicas ---

        private int _id;
        private string _codigo = string.Empty;
        private string _nombre = string.Empty;
        private decimal _precio;
        private int _stock;
        private string _descripcion = string.Empty;
        private string _imagen = string.Empty;

        public int Id
        {
            get => _id;
            set => _id = value > 0 ? value : throw new ArgumentException("El ID debe ser positivo.");
        }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código no puede superar 20 caracteres.")]
        public string Codigo
        {
            get => _codigo;
            set => _codigo = !string.IsNullOrWhiteSpace(value)
                ? value.ToUpper().Trim()
                : throw new ArgumentException("El código no puede estar vacío.");
        }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre
        {
            get => _nombre;
            set => _nombre = !string.IsNullOrWhiteSpace(value)
                ? value.Trim()
                : throw new ArgumentException("El nombre no puede estar vacío.");
        }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999,999.99.")]
        public decimal Precio
        {
            get => _precio;
            set => _precio = value > 0
                ? value
                : throw new ArgumentException("El precio debe ser mayor a 0.");
        }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 9999, ErrorMessage = "El stock debe estar entre 0 y 9999.")]
        public int Stock
        {
            get => _stock;
            set => _stock = value >= 0
                ? value
                : throw new ArgumentException("El stock no puede ser negativo.");
        }

        public string Descripcion
        {
            get => _descripcion;
            set => _descripcion = value?.Trim() ?? string.Empty;
        }

        public string Imagen
        {
            get => _imagen;
            set => _imagen = value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Tipo de producto para diferenciar en vistas (Polimorfismo).
        /// </summary>
        public abstract string TipoProducto { get; }

        /// <summary>
        /// Ícono visual representativo del tipo.
        /// </summary>
        public abstract string IconoTipo { get; }

        /// <summary>
        /// Color de badge para diferenciación visual.
        /// </summary>
        public abstract string ColorTipo { get; }

        /// <summary>
        /// Método polimórfico: cada tipo devuelve su descripción detallada propia.
        /// </summary>
        public abstract string ObtenerDescripcionDetallada();

        /// <summary>
        /// Verifica si hay stock disponible (Encapsulación del control de stock).
        /// </summary>
        public bool HayStock() => _stock > 0;

        /// <summary>
        /// Reduce el stock al realizar una venta (Encapsulación).
        /// </summary>
        public bool ReducirStock(int cantidad)
        {
            if (cantidad <= 0) return false;
            if (_stock < cantidad) return false;
            _stock -= cantidad;
            return true;
        }

        /// <summary>
        /// Reabastece el stock (Encapsulación).
        /// </summary>
        public void ReabastecerStock(int cantidad)
        {
            if (cantidad > 0)
                _stock += cantidad;
        }

        /// <summary>
        /// Muestra información general del producto (base para polimorfismo).
        /// </summary>
        public virtual string MostrarInfo()
        {
            return $"[{TipoProducto}] {Nombre} | Código: {Codigo} | Precio: ${Precio:N2} | Stock: {Stock}";
        }
    }
}