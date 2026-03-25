using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Pedidos
{
    public class IndexModel : PageModel
    {
        private readonly TiendaService _tiendaService;
        public IndexModel(TiendaService tiendaService) => _tiendaService = tiendaService;

        public List<Pedido> Pedidos { get; set; } = new();
        public decimal TotalVentas { get; set; }

        public void OnGet()
        {
            Pedidos = _tiendaService.ObtenerPedidos();
            TotalVentas = Pedidos.Sum(p => p.Total);
        }
    }
}