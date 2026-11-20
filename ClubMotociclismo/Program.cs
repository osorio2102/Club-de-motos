using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ClubMotociclismo.Data;

// Usamos el formato de Colombia para los precios (ej: 60.000)
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-CO");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registramos el DbContext con la cadena de conexión de SQL Server (appsettings.json)
builder.Services.AddDbContext<ClubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClubConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
