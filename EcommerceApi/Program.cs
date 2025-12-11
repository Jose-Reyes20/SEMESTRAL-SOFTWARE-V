using Microsoft.EntityFrameworkCore; // Requerido para DBContext y MySQL
using EcommerceApi.Data; // Requerido para ApplicationDbContext
using EcommerceApi.Services; // Necesario para acceder a ArticuloService y AuthService (tal como lo definiste)
using EcommerceApi.Services.Interfaces; // Requerido para inyectar los servicios I...Service

var builder = WebApplication.CreateBuilder(args);

// ***************************************************************
// 1. CONFIGURACIÓN DE SERVICIOS
// ***************************************************************

// 1.1 Conexión a MySQL (Entity Framework Core)
// NOTA: Asegúrate de que la cadena de conexión 'DefaultConnection' esté definida en appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Paquete NuGet requerido: Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// 1.2 INYECCIÓN DE DEPENDENCIA DE SERVICIOS CORE (Base de datos)
// Usamos AddScoped para los servicios que interactúan con el DBContext
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IOrdenesService, OrdenesService>();
builder.Services.AddScoped<IFacturasService, FacturasService>();
builder.Services.AddScoped<ICuponesService, CuponesService>();

// 1.3 INYECCIÓN DE DEPENDENCIA DE SERVICIOS DE SIMULACIÓN (del snippet que proveiste)
// Usamos AddSingleton/AddScoped/AddTransient dependiendo del ciclo de vida.
builder.Services.AddSingleton<ArticuloService>();
builder.Services.AddSingleton<AuthService>();

builder.Services.AddControllers();

// Opcional: Configuración de Swagger/OpenAPI para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ***************************************************************
// 2. CONSTRUCCIÓN DE LA APLICACIÓN
// ***************************************************************

var app = builder.Build();


// ***************************************************************
// 3. MIDDLEWARE (HTTP request pipeline)
// ***************************************************************

// Configuración de Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Asegúrate de que el middleware de enrutamiento esté antes de MapControllers
app.UseRouting();

app.UseAuthorization(); // Usar autenticación y autorización

app.MapControllers(); // Mapea los controladores (endpoints)

app.Run();