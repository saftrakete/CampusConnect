using CampusConnect.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

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

var app = builder.Build();

app.UseCors("allowOrigin");

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<CampusConnectContext>();
context.Database.Migrate();

app.Run();
