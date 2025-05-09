using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Models;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CampusConnectContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CampusConnectDb"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "allowOrigin",
        policy =>
        {
            policy.WithOrigins("https://127.0.0.1:4200");
            policy.AllowCredentials();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("allowOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<CampusConnectContext>();
context.Database.Migrate();

if (!context.UserRoles.Any(role => role.RoleName == "Admin"))
{
    var role = new UserRole
    {
        RoleName = "Admin",
        RoleDescription = "Admin-Rolle mit allen Rechten",
        Permissions = new List<string>()
        {
            "Lesen",
            "Schreiben",
            "Löschen"
        }
    };

    context.UserRoles.Add(role);
    context.SaveChanges();
}

app.Run();
