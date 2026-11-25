WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Orígenes desde donde se abre la UI (Vite o Live Server); se leen de appsettings.json
string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("UI", policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Las rutas y los servicios de destino están en la sección ReverseProxy de appsettings.json
builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

WebApplication app = builder.Build();

// CORS va antes del proxy para que responda las peticiones OPTIONS del navegador
app.UseCors("UI");

app.MapReverseProxy().RequireCors("UI");

app.Run();