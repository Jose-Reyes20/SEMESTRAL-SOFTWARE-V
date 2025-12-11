using EcommerceApi.Services; // Necesario para acceder a ArticuloService y AuthService

var builder = WebApplication.CreateBuilder(args);

// ***************************************************************
// 1. CONFIGURACIÓN DE SERVICIOS
// ***************************************************************

// 1.1 Agregar el servicio de artículos (Inventario)
// Usamos AddSingleton/AddScoped/AddTransient dependiendo del ciclo de vida. 
// Para este ejemplo de simulación, AddSingleton es suficiente.
builder.Services.AddSingleton<ArticuloService>();

// 1.2 Agregar el servicio de autenticación (Clientes y Usuarios)
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