using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Obtiene el estado de salud general de la aplicación
    /// </summary>
    /// <returns>Estado de salud de todos los componentes del sistema</returns>
    /// <response code="200">El sistema está saludable</response>
    /// <response code="503">Uno o más componentes no están saludables</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.ToString(),
                exception = entry.Value.Exception?.Message
            }),
            totalDuration = report.TotalDuration.ToString()
        };

        return report.Status == HealthStatus.Healthy ? Ok(result) : StatusCode(503, result);
    }

    /// <summary>
    /// Verifica el estado de la conexión con MongoDB
    /// </summary>
    /// <returns>Estado de salud de la conexión con MongoDB</returns>
    /// <response code="200">La conexión con MongoDB está saludable</response>
    /// <response code="503">Hay problemas con la conexión a MongoDB</response>
    [HttpGet("mongodb")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetMongoDbHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync(c => c.Tags.Contains("mongodb"));
        
        var mongoCheck = report.Entries.FirstOrDefault();
        var result = new
        {
            status = report.Status.ToString(),
            check = mongoCheck.Key == null ? null : new
            {
                name = mongoCheck.Key,
                status = mongoCheck.Value.Status.ToString(),
                description = mongoCheck.Value.Description,
                duration = mongoCheck.Value.Duration.ToString(),
                exception = mongoCheck.Value.Exception?.Message
            },
            totalDuration = report.TotalDuration.ToString()
        };

        return report.Status == HealthStatus.Healthy ? Ok(result) : StatusCode(503, result);
    }
}