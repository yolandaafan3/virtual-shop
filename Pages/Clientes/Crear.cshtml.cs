using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Clientes
{
    public class CrearModel : PageModel
    {
        private readonly TiendaService _tiendaService;
        public CrearModel(TiendaService tiendaService) => _tiendaService = tiendaService;

        [BindProperty] public Cliente Cliente { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // SOLUCIÓN AL ERROR: Le decimos a ASP.NET que ignore el campo Codigo 
            // durante la validación del formulario, ya que lo generaremos en el backend.
            ModelState.Remove("Cliente.Codigo");

            if (!ModelState.IsValid) return Page();

            if (_tiendaService.EmailExiste(Cliente.Email))
            {
                ModelState.AddModelError("Cliente.Email", "Este email ya está registrado.");
                return Page();
            }

            _tiendaService.AgregarCliente(Cliente);
            TempData["Exito"] = $"Cliente '{Cliente.NombreCompleto}' registrado. Código: {Cliente.Codigo}";
            return RedirectToPage("/Clientes/Index");
        }
    }
}