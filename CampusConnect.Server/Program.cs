using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Services
builder.Services.AddTransient<InitModuleTable>();
builder.Services.AddTransient<InitFacultyTable>();
builder.Services.AddTransient<InitDegreeTable>();
builder.Services.AddTransient<InitUserRolesService>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<ITwoFactorService, TwoFactorService>();

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
            policy.WithOrigins(
                "https://127.0.0.1:4200",
                "https://localhost:4200",
                "http://127.0.0.1:4200",
                "http://localhost:4200"
            )
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
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

var app = builder.Build();

app.UseCors("allowOrigin");

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

if (!app.Environment.IsEnvironment("Testing"))
{
    var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<CampusConnectContext>();
    var moduleIntitializer = scope.ServiceProvider.GetRequiredService<InitModuleTable>();
    var facultyInitializer = scope.ServiceProvider.GetRequiredService<InitFacultyTable>();
    var degreeInitializer = scope.ServiceProvider.GetRequiredService<InitDegreeTable>();
    var userRolesInitializer = scope.ServiceProvider.GetRequiredService<InitUserRolesService>();

    context.Database.Migrate();

    await facultyInitializer.FillInFaculties();
    await moduleIntitializer.FillInModules();
    await degreeInitializer.FillInDegrees();

    userRolesInitializer.InitUserRolesTable(context);
}

app.Run();

public partial class Program { }
