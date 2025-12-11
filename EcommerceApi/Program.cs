using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.Services;
using EcommerceApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ***************************************************************
// 1. CONFIGURACIÓN DE LA BASE DE DATOS
// ***************************************************************

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ***************************************************************
// 2. INYECCIÓN DE DEPENDENCIA DE SERVICIOS
// ***************************************************************

builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IOrdenesService, OrdenesService>();
builder.Services.AddScoped<IFacturasService, FacturasService>();
builder.Services.AddScoped<ICuponesService, CuponesService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<AuthService>();

// ***************************************************************
// 3. CONFIGURACIÓN DE CORS (NUEVO: Para conectar el Frontend)
// ***************************************************************
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()  // Permite que entre cualquiera (tu HTML)
           .AllowAnyHeader()
           .AllowAnyMethod(); // Permite GET, POST, PUT, DELETE
    });
});

// ***************************************************************
// 4. CONFIGURACIÓN DE LA API
// ***************************************************************

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ***************************************************************
// 5. MIDDLEWARE (Pipeline de Peticiones HTTP)
// ***************************************************************

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

// ACTIVAR LA POLÍTICA CORS (IMPORTANTE: Poner antes de Authorization)
app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();