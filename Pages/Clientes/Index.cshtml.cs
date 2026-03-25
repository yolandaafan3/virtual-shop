using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly TiendaService _tiendaService;
        public IndexModel(TiendaService tiendaService) => _tiendaService = tiendaService;

        public List<Cliente> Clientes { get; set; } = new();
        public string Buscar { get; set; } = string.Empty;

        public void OnGet(string? buscar)
        {
            Buscar = buscar ?? string.Empty;
            var todos = _tiendaService.ObtenerClientes();
            if (!string.IsNullOrWhiteSpace(Buscar))
                Clientes = todos.Where(c =>
                    c.NombreCompleto.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(Buscar, StringComparison.OrdinalIgnoreCase) ||
                    c.Codigo.Contains(Buscar, StringComparison.OrdinalIgnoreCase)).ToList();
            else
                Clientes = todos;
        }
    }
}