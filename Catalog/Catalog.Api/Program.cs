using System.Security.Claims;                                 
using System.Text;                                            
using Catalog.Application.Extensions;
using Catalog.Persistence.Extensions;
using Catalog.Persistence.Seeds;
using Catalog.Persistence.Seeds.Licensing;
using Microsoft.AspNetCore.Authentication.JwtBearer;           
using Microsoft.IdentityModel.Tokens;                          

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- NUEVO: validación del JWT emitido por Identity ----
IConfigurationSection jwt = builder.Configuration.GetSection("JWT");

string signKey = jwt["SignKey"] ?? throw new InvalidOperationException("Falta la configuración JWT:SignKey.");

SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwt["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name,
                    };
                });

builder.Services.AddAuthorization();
// ---- fin de lo nuevo ----

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

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();