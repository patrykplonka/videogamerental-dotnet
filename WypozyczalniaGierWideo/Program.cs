using WypozyczalniaGier.Data;
using WypozyczalniaGier.Repositories;
using WypozyczalniaGier.Services; // Dodaj import dla serwisu
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodaj kontekst bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytoriów
builder.Services.AddScoped<IWypozyczenieRepository, WypozyczenieRepository>();
builder.Services.AddScoped<IUzytkownikRepository, UzytkownikRepository>();
builder.Services.AddScoped<IGraRepository, GraRepository>();

// Rejestracja serwisu IGraService
builder.Services.AddScoped<IGraService, GraService>();
builder.Services.AddScoped<IUzytkownikService, UzytkownikService>();
builder.Services.AddScoped<IWypozyczenieService,WypozyczenieService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Konfiguracja middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
