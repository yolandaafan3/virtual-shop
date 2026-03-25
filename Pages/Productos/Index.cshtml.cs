using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Productos
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

        public List<Producto> Productos { get; set; } = new();
        public string TipoFiltro { get; set; } = string.Empty;
        public string Buscar { get; set; } = string.Empty;
        public bool SoloConStock { get; set; }

        public void OnGet(string? tipo, string? buscar, bool soloConStock = false)
        {
            TipoFiltro = tipo ?? string.Empty;
            Buscar = buscar ?? string.Empty;
            SoloConStock = soloConStock;

            var query = string.IsNullOrEmpty(tipo)
                ? _tiendaService.ObtenerProductos()
                : _tiendaService.ObtenerProductosPorTipo(tipo);

            if (!string.IsNullOrWhiteSpace(Buscar))
                query = query.Where(p =>
                    p.Nombre.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ||
                    p.Codigo.Contains(Buscar, StringComparison.OrdinalIgnoreCase)).ToList();

            if (soloConStock)
                query = query.Where(p => p.HayStock()).ToList();

            Productos = query;
        }

        public IActionResult OnPostAgregarCarrito(int productoId)
        {
            var producto = _tiendaService.ObtenerProductoPorId(productoId);
            if (producto == null) { TempData["Error"] = "Producto no encontrado."; return RedirectToPage(); }
            var (exito, mensaje) = _carritoService.AgregarItem(producto, 1);
            TempData[exito ? "Exito" : "Error"] = mensaje;
            return RedirectToPage();
        }

        public IActionResult OnPostEliminar(int id)
        {
            bool eliminado = _tiendaService.EliminarProducto(id);
            TempData[eliminado ? "Exito" : "Error"] = eliminado ? "Producto eliminado." : "No se pudo eliminar.";
            return RedirectToPage();
        }
    }
}