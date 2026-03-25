using TiendaVirtual.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Razor Pages
builder.Services.AddRazorPages();

// Registrar servicios como Singleton (datos en memoria compartidos)
builder.Services.AddSingleton<TiendaService>();

// SOLUCIÓN: Cambiado a Singleton. 
// En una app real usaríamos IHttpContextAccessor y Session, pero para 
// mantener el diseño POO en memoria puro, Singleton mantiene el carrito vivo.
builder.Services.AddSingleton<CarritoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();