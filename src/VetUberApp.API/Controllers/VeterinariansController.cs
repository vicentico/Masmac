using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class VeterinariansController : ControllerBase
{
    private readonly VeterinarianService _veterinarianService;

    public VeterinariansController(VeterinarianService veterinarianService)
    {
        _veterinarianService = veterinarianService;
    }

    /// <summary>
    /// Crea un nuevo veterinario en el sistema
    /// </summary>
    /// <param name="dto">Datos del veterinario a crear</param>
    /// <returns>El veterinario creado con su ID asignado</returns>
    /// <response code="201">Veterinario creado exitosamente</response>
    /// <response code="400">Email ya existe o datos inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VeterinarianDto>> Create([FromBody] CreateVeterinarianDto dto)
    {
        try
        {
            var veterinarian = await _veterinarianService.CreateAsync(dto);
            return Created($"/api/veterinarians/{veterinarian.Id}", veterinarian);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un veterinario por su ID
    /// </summary>
    /// <param name="id">ID del veterinario</param>
    /// <returns>Datos del veterinario</returns>
    /// <response code="200">Veterinario encontrado</response>
    /// <response code="404">Veterinario no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeterinarianDto>> GetById(string id)
    {
        var veterinarian = await _veterinarianService.GetByIdAsync(id);
        if (veterinarian == null)
            return NotFound();

        return veterinarian;
    }

    /// <summary>
    /// Obtiene todos los veterinarios del sistema
    /// </summary>
    /// <returns>Lista de veterinarios</returns>
    /// <response code="200">Lista de veterinarios obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetAll()
    {
        var veterinarians = await _veterinarianService.GetAllAsync();
        return Ok(veterinarians);
    }

    /// <summary>
    /// Obtiene los veterinarios disponibles
    /// </summary>
    /// <returns>Lista de veterinarios disponibles</returns>
    /// <response code="200">Lista de veterinarios disponibles obtenida exitosamente</response>
    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetAvailable()
    {
        var veterinarians = await _veterinarianService.GetAvailableAsync();
        return Ok(veterinarians);
    }

    /// <summary>
    /// Busca veterinarios por especialidad
    /// </summary>
    /// <param name="specialty">Especialidad a buscar</param>
    /// <returns>Lista de veterinarios con la especialidad especificada</returns>
    /// <response code="200">Búsqueda realizada exitosamente</response>
    [HttpGet("specialty/{specialty}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetBySpecialty(string specialty)
    {
        var veterinarians = await _veterinarianService.FindBySpecialtyAsync(specialty);
        return Ok(veterinarians);
    }

    /// <summary>
    /// Actualiza los datos de un veterinario existente
    /// </summary>
    /// <param name="id">ID del veterinario a actualizar</param>
    /// <param name="dto">Nuevos datos del veterinario</param>
    /// <returns>Veterinario actualizado</returns>
    /// <response code="200">Veterinario actualizado exitosamente</response>
    /// <response code="404">Veterinario no encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeterinarianDto>> Update(string id, [FromBody] UpdateVeterinarianDto dto)
    {
        try
        {
            var veterinarian = await _veterinarianService.UpdateAsync(id, dto);
            return Ok(veterinarian);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza la ubicación de un veterinario
    /// </summary>
    /// <param name="id">ID del veterinario</param>
    /// <param name="latitude">Nueva latitud</param>
    /// <param name="longitude">Nueva longitud</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Ubicación actualizada exitosamente</response>
    [HttpPut("{id}/location")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateLocation(string id, [FromQuery] double latitude, [FromQuery] double longitude)
    {
        await _veterinarianService.UpdateLocationAsync(id, latitude, longitude);
        return NoContent();
    }

    /// <summary>
    /// Actualiza la disponibilidad de un veterinario
    /// </summary>
    /// <param name="id">ID del veterinario</param>
    /// <param name="isAvailable">Nueva disponibilidad</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Disponibilidad actualizada exitosamente</response>
    [HttpPut("{id}/availability")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAvailability(string id, [FromQuery] bool isAvailable)
    {
        await _veterinarianService.UpdateAvailabilityAsync(id, isAvailable);
        return NoContent();
    }

    /// <summary>
    /// Elimina un veterinario del sistema (soft delete)
    /// </summary>
    /// <param name="id">ID del veterinario a eliminar</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Veterinario eliminado exitosamente</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(string id)
    {
        await _veterinarianService.DeleteAsync(id);
        return NoContent();
    }
}