using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Productos
{
    public class DetalleModel : PageModel
    {
        private readonly TiendaService _tiendaService;
        private readonly CarritoService _carritoService;

        public DetalleModel(TiendaService tiendaService, CarritoService carritoService)
        {
            _tiendaService = tiendaService;
            _carritoService = carritoService;
        }

        public Producto? Producto { get; set; }

        public void OnGet(int id)
        {
            Producto = _tiendaService.ObtenerProductoPorId(id);
        }

        public IActionResult OnPostAgregarCarrito(int productoId, int cantidad = 1)
        {
            var producto = _tiendaService.ObtenerProductoPorId(productoId);
            if (producto == null) { TempData["Error"] = "Producto no encontrado."; return RedirectToPage(new { id = productoId }); }

            var (exito, mensaje) = _carritoService.AgregarItem(producto, cantidad);
            TempData[exito ? "Exito" : "Error"] = mensaje;
            return RedirectToPage(new { id = productoId });
        }
    }
}