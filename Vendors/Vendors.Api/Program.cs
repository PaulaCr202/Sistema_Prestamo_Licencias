using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Vendors.Api.Middleware;
using Vendors.Application;
using Vendors.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- validación del JWT emitido por Identity (debe coincidir exactamente) ----
IConfigurationSection jwt = builder.Configuration.GetSection("JWT");

string signKey = jwt["SignKey"]
    ?? throw new InvalidOperationException("Falta la configuración JWT:SignKey.");

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
// ---- fin JWT ----

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(
    builder.Configuration.GetConnectionString("VendorsDb")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();