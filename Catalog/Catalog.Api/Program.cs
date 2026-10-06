using Catalog.Application.Extensions;
using Catalog.Persistence.Extensions;
using Catalog.Persistence.Seeds;
using Catalog.Persistence.Seeds.Licensing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(
    builder.Configuration.GetConnectionString("CatalogDb")!);

builder.Services.AddScoped<IDataSeeder, CategorySeeder>();
builder.Services.AddScoped<IDataSeeder, SoftwareSeeder>();
builder.Services.AddScoped<IDataSeeder, LicenseSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

await DataBaseSeeder.SeedAsync(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();