using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.Services;
using EcommerceApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ***************************************************************
// 1. CONFIGURACIÓN DE LA BASE DE DATOS
// ***************************************************************

// AQUI ESTÁ EL CAMBIO: Busca "MySqlConnection" en lugar de "DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Configuración de Entity Framework Core con MySQL (Pomelo)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ***************************************************************
// 2. INYECCIÓN DE DEPENDENCIA DE SERVICIOS
// ***************************************************************

// Servicios Core (Ya estaban bien)
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IOrdenesService, OrdenesService>();
builder.Services.AddScoped<IFacturasService, FacturasService>();
builder.Services.AddScoped<ICuponesService, CuponesService>();

// Servicios de Artículos y Autenticación
// CAMBIO IMPORTANTE: Se cambian a 'AddScoped' porque ahora usan la base de datos.
// Si los dejas como 'Singleton' te darán un error al intentar acceder al DBContext.
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<AuthService>();

// ***************************************************************
// 3. CONFIGURACIÓN DE LA API
// ***************************************************************

builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ***************************************************************
// 4. MIDDLEWARE (Pipeline de Peticiones HTTP)
// ***************************************************************

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();