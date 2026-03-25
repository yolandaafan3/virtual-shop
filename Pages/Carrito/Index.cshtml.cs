using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Carrito
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

        public List<ItemCarrito> Items { get; set; } = new();
        public List<Cliente> Clientes { get; set; } = new();
        public decimal Total { get; set; }
        public int CantidadTotal { get; set; }

        public void OnGet()
        {
            Items = _carritoService.ObtenerItems();
            Total = _carritoService.ObtenerTotal();
            CantidadTotal = _carritoService.ObtenerCantidadTotal();
            Clientes = _tiendaService.ObtenerClientes();
        }

        public IActionResult OnPostActualizarCantidad(int productoId, int cantidad)
        {
            _carritoService.ActualizarCantidad(productoId, cantidad);
            return RedirectToPage();
        }

        public IActionResult OnPostEliminar(int productoId)
        {
            _carritoService.EliminarItem(productoId);
            TempData["Exito"] = "Producto eliminado del carrito.";
            return RedirectToPage();
        }

        public IActionResult OnPostVaciar()
        {
            _carritoService.VaciarCarrito();
            TempData["Exito"] = "Carrito vaciado.";
            return RedirectToPage();
        }

        public IActionResult OnPostConfirmar(int clienteId, string notas = "")
        {
            var items = _carritoService.ObtenerItems();
            var (exito, mensaje, pedido) = _tiendaService.CrearPedido(clienteId, items, notas);

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return RedirectToPage();
            }

            _carritoService.VaciarCarrito();
            TempData["Exito"] = $"✅ Pedido {pedido!.NumeroPedido} confirmado por ${pedido.Total:N2}";
            return RedirectToPage("/Pedidos/Index");
        }
    }
}