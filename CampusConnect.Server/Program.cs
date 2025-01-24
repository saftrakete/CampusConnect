using CampusConnect.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            policy.WithOrigins("http://localhost:4200");
            policy.AllowCredentials();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("allowOrigin");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<CampusConnectContext>();
context.Database.Migrate();

app.Run();
