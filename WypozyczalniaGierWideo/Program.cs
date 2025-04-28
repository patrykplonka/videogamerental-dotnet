using WypozyczalniaGier.Data;
using WypozyczalniaGier.Repositories;
using WypozyczalniaGier.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Baza danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytoriów i serwisów
builder.Services.AddScoped<IWypozyczenieRepository, WypozyczenieRepository>();
builder.Services.AddScoped<IUzytkownikRepository, UzytkownikRepository>();
builder.Services.AddScoped<IGraRepository, GraRepository>();
builder.Services.AddScoped<IGraService, GraService>();
builder.Services.AddScoped<IUzytkownikService, UzytkownikService>();
builder.Services.AddScoped<IWypozyczenieService, WypozyczenieService>();

// MVC
builder.Services.AddControllersWithViews();

// ✅ Nowoczesna rejestracja FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<WypozyczalniaGier.ViewModels.GraViewModel>();

var app = builder.Build();

// Middleware
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
