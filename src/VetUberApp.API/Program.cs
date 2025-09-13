using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Infrastructure;
using VetUberApp.Infrastructure.Persistence.Configurations;
using VetUberApp.API.Filters;
using VetUberApp.Application;
using VetUberApp.Application.Validators;
using FluentValidation;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();

// Add Application Layer
builder.Services.AddApplicationServices();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateReviewDtoValidator>();

// Add MongoDB Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
MongoDbConfiguration.Configure();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddMongoDb(
        builder.Configuration.GetConnectionString("MongoDB") ?? 
            throw new InvalidOperationException("MongoDB connection string not found."),
        "VetUberDB",
        name: "mongodb",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
        tags: new[] { "mongodb", "ready" });

// Configure HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7155;
});

// Configure Kestrel
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5233); // HTTP
    serverOptions.ListenLocalhost(7155, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "VetUber API", 
        Version = "v1",
        Description = "API para el servicio de veterinarios a domicilio"
    });

    // Habilitar comentarios XML para Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
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

// Map health check endpoints
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception?.Message,
                    duration = entry.Value.Duration.ToString()
                })
            });
        
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});

// Enable HTTPS redirection
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
