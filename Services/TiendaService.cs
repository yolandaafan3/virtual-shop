using TiendaVirtual.Models;

namespace TiendaVirtual.Services
{
    /// <summary>
    /// Servicio principal de la tienda. Gestiona productos, clientes y pedidos.
    /// Actúa como repositorio en memoria (simula base de datos).
    /// </summary>
    public class TiendaService
    {
        private readonly List<Producto> _productos = new();
        private readonly List<Cliente> _clientes = new();
        private readonly List<Pedido> _pedidos = new();
        private int _nextProductoId = 1;
        private int _nextClienteId = 1;
        private int _nextPedidoId = 1;

        public TiendaService()
        {
            SeedData();
        }

        // ===================== PRODUCTOS =====================

        public List<Producto> ObtenerProductos() => _productos.ToList();

        public List<Producto> ObtenerProductosPorTipo(string tipo)
        {
            return tipo switch
            {
                "Tecnología" => _productos.OfType<ProductoTecnologico>().Cast<Producto>().ToList(),
                "Ropa" => _productos.OfType<ProductoRopa>().Cast<Producto>().ToList(),
                "Alimento" => _productos.OfType<ProductoAlimento>().Cast<Producto>().ToList(),
                _ => _productos.ToList()
            };
        }

        public Producto? ObtenerProductoPorId(int id) =>
            _productos.FirstOrDefault(p => p.Id == id);

        public bool CodigoProductoExiste(string codigo, int? excludeId = null) =>
            _productos.Any(p => p.Codigo.Equals(codigo.ToUpper(), StringComparison.OrdinalIgnoreCase)
                                && p.Id != excludeId);

        public void AgregarProducto(Producto producto)
        {
            producto.Id = _nextProductoId++;
            _productos.Add(producto);
        }

        public bool EliminarProducto(int id)
        {
            var p = ObtenerProductoPorId(id);
            if (p == null) return false;
            _productos.Remove(p);
            return true;
        }

        public Dictionary<string, int> ObtenerEstadisticasPorTipo()
        {
            return new Dictionary<string, int>
            {
                ["Tecnología"] = _productos.OfType<ProductoTecnologico>().Count(),
                ["Ropa"] = _productos.OfType<ProductoRopa>().Count(),
                ["Alimento"] = _productos.OfType<ProductoAlimento>().Count()
            };
        }

        // ===================== CLIENTES =====================

        public List<Cliente> ObtenerClientes() => _clientes.ToList();

        public Cliente? ObtenerClientePorId(int id) =>
            _clientes.FirstOrDefault(c => c.Id == id);

        public Cliente? BuscarClientePorCodigo(string codigo) =>
            _clientes.FirstOrDefault(c => c.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

        public bool EmailExiste(string email) =>
            _clientes.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public void AgregarCliente(Cliente cliente)
        {
            cliente.Id = _nextClienteId;
            cliente.Codigo = $"CLI{_nextClienteId:D4}";
            cliente.FechaRegistro = DateTime.Now;
            _nextClienteId++;
            _clientes.Add(cliente);
        }

        // ===================== PEDIDOS =====================

        public List<Pedido> ObtenerPedidos() =>
            _pedidos.OrderByDescending(p => p.FechaPedido).ToList();

        public List<Pedido> ObtenerPedidosDeCliente(int clienteId) =>
            _pedidos.Where(p => p.ClienteId == clienteId)
                    .OrderByDescending(p => p.FechaPedido)
                    .ToList();

        public (bool exito, string mensaje, Pedido? pedido) CrearPedido(
            int clienteId, List<ItemCarrito> carrito, string notas = "")
        {
            var cliente = ObtenerClientePorId(clienteId);
            if (cliente == null) return (false, "Cliente no encontrado.", null);
            if (!carrito.Any()) return (false, "El carrito está vacío.", null);

            foreach (var item in carrito)
            {
                var producto = ObtenerProductoPorId(item.ProductoId);
                if (producto == null)
                    return (false, $"Producto '{item.NombreProducto}' no encontrado.", null);
                if (!producto.HayStock() || producto.Stock < item.Cantidad)
                    return (false, $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}.", null);
            }

            foreach (var item in carrito)
            {
                var producto = ObtenerProductoPorId(item.ProductoId);
                producto!.ReducirStock(item.Cantidad);
            }

            var pedido = new Pedido
            {
                Id = _nextPedidoId,
                NumeroPedido = $"PED-{_nextPedidoId:D6}",
                ClienteId = clienteId,
                NombreCliente = cliente.NombreCompleto,
                FechaPedido = DateTime.Now,
                Estado = EstadoPedido.Completado,
                NotasAdicionales = notas,
                Items = carrito.Select(i => new ItemPedido
                {
                    ProductoId = i.ProductoId,
                    NombreProducto = i.NombreProducto,
                    TipoProducto = i.TipoProducto,
                    IconoTipo = i.IconoTipo,
                    PrecioUnitario = i.PrecioUnitario,
                    Cantidad = i.Cantidad
                }).ToList()
            };

            cliente.TotalPedidos++;
            _nextPedidoId++;
            _pedidos.Add(pedido);

            return (true, "Pedido creado exitosamente.", pedido);
        }

        // ===================== DATOS SEMILLA =====================

        private void SeedData()
        {
            // --- CONSUMIDOR FINAL (Venta Rápida) ---
            AgregarCliente(new Cliente 
            { 
                Nombre = "Consumidor", 
                Apellido = "Final", 
                Email = "cf@nexstore.local", 
                Telefono = "N/A", 
                Direccion = "Venta de mostrador" 
            });

            // --- 20 PRODUCTOS TECNOLÓGICOS ---
            var techData = new[] {
                ("Laptop ProBook 15", 899.99m, "💻", "TechPro", "Intel i7, 16GB RAM, 512GB SSD"),
                ("Auriculares Bluetooth Pro", 129.99m, "🎧", "SoundMax", "40h batería, ANC"),
                ("Smartphone UltraX", 599.99m, "📱", "UltraTech", "128GB, 5G, Triple cámara"),
                ("Monitor 4K 27 Pulgadas", 349.99m, "🖥️", "VisionX", "IPS, 144Hz, 1ms"),
                ("Teclado Mecánico RGB", 89.99m, "⌨️", "Clicky", "Switches Cherry MX Red"),
                ("Ratón Inalámbrico Ergo", 49.99m, "🖱️", "Clicky", "10000 DPI, Batería 30 días"),
                ("Smartwatch Fitness Plus", 199.99m, "⌚", "FitTime", "Monitor cardíaco, GPS, 5ATM"),
                ("Tablet OctaCore 10\"", 299.99m, "📱", "UltraTech", "Pantalla 2K, 64GB, 4GB RAM"),
                ("Disco Duro Externo 2TB", 79.99m, "💾", "DataSafe", "USB 3.2, Resistente a golpes"),
                ("SSD NVMe 1TB", 109.99m, "💽", "DataSafe", "Lectura 3500MB/s, Escritura 3000MB/s"),
                ("Tarjeta Gráfica RTX 4060", 499.99m, "🎮", "GameVision", "8GB GDDR6, Ray Tracing"),
                ("Memoria RAM 32GB DDR5", 149.99m, "🧠", "SpeedData", "6000MHz, CL30, RGB"),
                ("Cámara Mirrorless 4K", 899.99m, "📷", "PhotoGen", "Sensor APS-C, Lente 18-55mm"),
                ("Altavoz Inteligente", 59.99m, "🔊", "SoundMax", "Asistente de voz, WiFi, Bluetooth"),
                ("Consola de Videojuegos", 499.99m, "🎮", "GameVision", "NextGen, 4K a 120fps"),
                ("Gafas de Realidad Virtual", 399.99m, "🥽", "Virtuality", "Resolución 4K, Controladores hápticos"),
                ("Router WiFi 6 Mesh", 129.99m, "📡", "NetConnect", "Cobertura 300m2, Tribanda"),
                ("Batería Portátil 20000mAh", 39.99m, "🔋", "PowerUp", "Carga rápida 20W, 3 puertos"),
                ("Micrófono de Condensador USB", 89.99m, "🎙️", "SoundMax", "Patrón cardioide, Soporte incluido"),
                ("Lector de Libros Electrónicos", 119.99m, "📖", "ReadIt", "Pantalla E-Ink 6\", Luz cálida ajust.")
            };

            for (int i = 0; i < techData.Length; i++)
            {
                AgregarProducto(new ProductoTecnologico
                {
                    Codigo = $"TECH{i + 1:D3}",
                    Nombre = techData[i].Item1,
                    Precio = techData[i].Item2,
                    Stock = new Random().Next(10, 50),
                    Descripcion = $"Excelente {techData[i].Item1.ToLower()} para uso diario.",
                    Imagen = techData[i].Item3,
                    Marca = techData[i].Item4,
                    Modelo = $"Model-{i + 1}",
                    GarantiaAnios = new Random().Next(1, 4),
                    Especificaciones = techData[i].Item5
                });
            }

            // --- 20 PRODUCTOS DE ROPA ---
            var ropaData = new[] {
                ("Camiseta Casual Premium", 24.99m, "👕", "Algodón", "Unisex"),
                ("Jeans Slim Fit", 49.99m, "👖", "Denim", "Hombre"),
                ("Chaqueta de Cuero", 129.99m, "🧥", "Cuero Sintético", "Mujer"),
                ("Zapatillas Deportivas", 89.99m, "👟", "Malla/Goma", "Unisex"),
                ("Calcetines (Pack 3)", 12.99m, "🧦", "Algodón/Elastano", "Unisex"),
                ("Suéter de Lana Invierno", 59.99m, "🧶", "Lana 100%", "Unisex"),
                ("Pantalones Cortos Verano", 29.99m, "🩳", "Lino", "Hombre"),
                ("Vestido de Noche Elegante", 149.99m, "👗", "Seda", "Mujer"),
                ("Gorra de Béisbol Ajustable", 19.99m, "🧢", "Poliéster", "Unisex"),
                ("Bufanda de Punto Fina", 22.99m, "🧣", "Lana/Acrílico", "Unisex"),
                ("Camisa de Botones Formal", 39.99m, "👔", "Algodón/Poliéster", "Hombre"),
                ("Falda Plisada Midi", 34.99m, "👗", "Poliéster", "Mujer"),
                ("Abrigo Impermeable", 89.99m, "🧥", "Nylon", "Unisex"),
                ("Traje de Baño Deportivo", 29.99m, "🩱", "Lycra", "Mujer"),
                ("Bañador Tipo Short", 24.99m, "🩲", "Nylon", "Hombre"),
                ("Cinturón de Cuero Reversible", 29.99m, "🟤", "Cuero Real", "Hombre"),
                ("Guantes Táctiles Invierno", 15.99m, "🧤", "Poliéster/Spandex", "Unisex"),
                ("Corbata de Seda Estampada", 25.99m, "👔", "Seda 100%", "Hombre"),
                ("Blusa de Seda Cuello V", 45.99m, "👚", "Seda", "Mujer"),
                ("Leggings Alta Compresión", 35.99m, "👖", "Spandex/Nylon", "Mujer")
            };

            for (int i = 0; i < ropaData.Length; i++)
            {
                AgregarProducto(new ProductoRopa
                {
                    Codigo = $"ROPA{i + 1:D3}",
                    Nombre = ropaData[i].Item1,
                    Precio = ropaData[i].Item2,
                    Stock = new Random().Next(20, 100),
                    Descripcion = "Prenda cómoda y con excelentes acabados.",
                    Imagen = ropaData[i].Item3,
                    Talla = new[] { "S", "M", "L", "XL" }[new Random().Next(0, 4)],
                    Color = new[] { "Negro", "Azul", "Blanco", "Rojo", "Gris" }[new Random().Next(0, 5)],
                    Material = ropaData[i].Item4,
                    Genero = ropaData[i].Item5
                });
            }

            // --- 20 PRODUCTOS ALIMENTICIOS ---
            var alimData = new[] {
                ("Aceite de Oliva Extra Virgen", 12.99m, "🫒", "500ml", true),
                ("Granola Natural Frutos Rojos", 8.50m, "🥣", "400g", false),
                ("Café Tostado en Grano", 14.99m, "☕", "500g", false),
                ("Té Verde Matcha Orgánico", 19.99m, "🍵", "100g", false),
                ("Pasta de Trigo Duro", 2.99m, "🍝", "500g", false),
                ("Arroz Basmati Premium", 4.99m, "🍚", "1kg", false),
                ("Frijoles Negros Enteros", 3.50m, "🫘", "1kg", false),
                ("Chocolate Negro 70% Cacao", 5.99m, "🍫", "150g", false),
                ("Miel de Abeja Pura", 9.99m, "🍯", "500g", false),
                ("Mermelada de Fresa Artesanal", 6.50m, "🍓", "300g", true),
                ("Atún en Agua (Lata)", 1.99m, "🐟", "140g", false),
                ("Leche de Almendras", 3.99m, "🥛", "1L", true),
                ("Queso Parmesano Rallado", 7.99m, "🧀", "200g", true),
                ("Galletas de Avena y Pasas", 4.50m, "🍪", "250g", false),
                ("Jugo de Naranja Natural", 3.99m, "🧃", "1L", true),
                ("Salsa de Tomate Casera", 4.99m, "🍅", "400g", false),
                ("Vinagre Balsámico", 8.99m, "🏺", "250ml", false),
                ("Sal del Himalaya", 5.50m, "🧂", "500g", false),
                ("Pimienta Negra Molida", 4.25m, "🌶️", "100g", false),
                ("Quinoa Orgánica", 7.99m, "🌾", "500g", false)
            };

            for (int i = 0; i < alimData.Length; i++)
            {
                AgregarProducto(new ProductoAlimento
                {
                    Codigo = $"ALIM{i + 1:D3}",
                    Nombre = alimData[i].Item1,
                    Precio = alimData[i].Item2,
                    Stock = new Random().Next(30, 200),
                    Descripcion = "Producto fresco y de la mejor calidad para tu despensa.",
                    Imagen = alimData[i].Item3,
                    FechaVencimiento = DateTime.Now.AddDays(new Random().Next(30, 365)),
                    PesoVolumen = alimData[i].Item4,
                    EsOrganico = new Random().Next(0, 2) == 1,
                    RequiereRefrigeracion = alimData[i].Item5,
                    Ingredientes = "Ingredientes naturales y seleccionados."
                });
            }

            // --- CLIENTES REGULARES ---
            AgregarCliente(new Cliente { Nombre = "María", Apellido = "González", Email = "maria@email.com", Telefono = "555-0101", Direccion = "Calle 1, Ciudad" });
            AgregarCliente(new Cliente { Nombre = "Carlos", Apellido = "Rodríguez", Email = "carlos@email.com", Telefono = "555-0202", Direccion = "Av. Central 45" });
            AgregarCliente(new Cliente { Nombre = "Laura", Apellido = "Martínez", Email = "laura@email.com", Telefono = "555-0303", Direccion = "Residencial Las Flores" });
        }
    }
}