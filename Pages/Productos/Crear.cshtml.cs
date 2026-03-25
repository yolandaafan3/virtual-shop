using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiendaVirtual.Models;
using TiendaVirtual.Services;

namespace TiendaVirtual.Pages.Productos
{
    public class CrearModel : PageModel
    {
        private readonly TiendaService _tiendaService;

        public CrearModel(TiendaService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        [BindProperty] public ProductoTecnologico Tech { get; set; } = new();
        [BindProperty] public ProductoRopa Ropa { get; set; } = new();
        [BindProperty] public ProductoAlimento Alimento { get; set; } = new();

        public void OnGet() { }

        /// <summary>
        /// Handler para crear producto tecnológico (Polimorfismo en acción).
        /// </summary>
        public IActionResult OnPostCrearTecnologico()
        {
            // Validar solo el modelo Tech
            var validacion = ValidarProducto(Tech, "Tech");
            if (validacion != null) return validacion;

            if (_tiendaService.CodigoProductoExiste(Tech.Codigo))
            {
                ModelState.AddModelError("Tech.Codigo", "Este código ya existe.");
                return Page();
            }

            _tiendaService.AgregarProducto(Tech);
            TempData["Exito"] = $"Producto '{Tech.Nombre}' registrado correctamente.";
            return RedirectToPage("/Productos/Index");
        }

        /// <summary>
        /// Handler para crear producto de ropa.
        /// </summary>
        public IActionResult OnPostCrearRopa()
        {
            var validacion = ValidarProducto(Ropa, "Ropa");
            if (validacion != null) return validacion;

            if (_tiendaService.CodigoProductoExiste(Ropa.Codigo))
            {
                ModelState.AddModelError("Ropa.Codigo", "Este código ya existe.");
                return Page();
            }

            _tiendaService.AgregarProducto(Ropa);
            TempData["Exito"] = $"Producto '{Ropa.Nombre}' registrado correctamente.";
            return RedirectToPage("/Productos/Index");
        }

        /// <summary>
        /// Handler para crear producto alimenticio.
        /// </summary>
        public IActionResult OnPostCrearAlimento()
        {
            // Validar fecha
            if (Alimento.FechaVencimiento < DateTime.Now)
            {
                ModelState.AddModelError("Alimento.FechaVencimiento", "La fecha de vencimiento no puede ser pasada.");
                return Page();
            }

            var validacion = ValidarProducto(Alimento, "Alimento");
            if (validacion != null) return validacion;

            if (_tiendaService.CodigoProductoExiste(Alimento.Codigo))
            {
                ModelState.AddModelError("Alimento.Codigo", "Este código ya existe.");
                return Page();
            }

            _tiendaService.AgregarProducto(Alimento);
            TempData["Exito"] = $"Producto '{Alimento.Nombre}' registrado correctamente.";
            return RedirectToPage("/Productos/Index");
        }

        /// <summary>
        /// Valida solo los campos del prefijo dado (evitar errores cruzados entre formularios).
        /// </summary>
        private IActionResult? ValidarProducto(Producto producto, string prefijo)
        {
            // Limpiar errores de los otros formularios
            var keys = ModelState.Keys.Where(k => !k.StartsWith(prefijo)).ToList();
            foreach (var key in keys) ModelState.Remove(key);

            if (!ModelState.IsValid) return Page();
            return null;
        }
    }
}