using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TiendaService _tiendaService;
        private readonly CarritoService _carritoService;

        public IndexModel(TiendaService tiendaService, CarritoService carritoService)
        {
            _tiendaService = tiendaService;
            _carritoService = carritoService;
        }

        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalPedidos { get; set; }
        public decimal VentasTotales { get; set; }
        public Dictionary<string, int> ConteoTipos { get; set; } = new();
        public List<Producto> ProductosRecientes { get; set; } = new();

        public void OnGet()
        {
            var productos = _tiendaService.ObtenerProductos();
            TotalProductos = productos.Count;
            TotalClientes = _tiendaService.ObtenerClientes().Count;
            var pedidos = _tiendaService.ObtenerPedidos();
            TotalPedidos = pedidos.Count;
            VentasTotales = pedidos.Sum(p => p.Total);
            ConteoTipos = _tiendaService.ObtenerEstadisticasPorTipo();
            ProductosRecientes = productos.TakeLast(6).Reverse().ToList();
        }

        public IActionResult OnPostAgregarCarrito(int productoId)
        {
            var producto = _tiendaService.ObtenerProductoPorId(productoId);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToPage();
            }

            var (exito, mensaje) = _carritoService.AgregarItem(producto, 1);
            TempData[exito ? "Exito" : "Error"] = mensaje;
            return RedirectToPage();
        }
    }
}