using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Infrastructure;
using VetUberApp.Infrastructure.Persistence.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add MongoDB Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
MongoDbConfiguration.Configure();
// Configure HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7155;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "VetUber API", 
        Version = "v1",
        Description = "API para el servicio de veterinarios a domicilio"
    });
});

// Add MongoDB Infrastructure (temporarily disabled)
//builder.Services.AddInfrastructure(builder.Configuration);
//MongoDbConfiguration.Configure();

// Configure HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7155;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VetUber API V1");
        c.RoutePrefix = "swagger";
    });
}

// Use HTTPS redirection before other middleware
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
