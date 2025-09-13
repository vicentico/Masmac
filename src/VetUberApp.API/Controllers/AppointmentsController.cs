using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;
using VetUberApp.Domain.Enums;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public AppointmentsController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Crea una nueva cita
    /// </summary>
    /// <param name="dto">Datos de la cita a crear</param>
    /// <returns>La cita creada</returns>
    /// <response code="201">Cita creada exitosamente</response>
    /// <response code="400">Datos inválidos o conflicto de horarios</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.CreateAsync(dto);
            return Created($"/api/appointments/{appointment.Id}", appointment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una cita por su ID
    /// </summary>
    /// <param name="id">ID de la cita</param>
    /// <returns>Datos de la cita</returns>
    /// <response code="200">Cita encontrada</response>
    /// <response code="404">Cita no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById(string id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null)
            return NotFound();

        return appointment;
    }

    /// <summary>
    /// Obtiene todas las citas
    /// </summary>
    /// <returns>Lista de citas</returns>
    /// <response code="200">Lista de citas obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(appointments);
    }

    /// <summary>
    /// Obtiene las citas de un veterinario específico
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Lista de citas del veterinario</returns>
    /// <response code="200">Lista de citas obtenida exitosamente</response>
    [HttpGet("veterinarian/{veterinarianId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByVeterinarian(string veterinarianId)
    {
        var appointments = await _appointmentService.GetByVeterinarianIdAsync(veterinarianId);
        return Ok(appointments);
    }

    /// <summary>
    /// Obtiene las citas de un dueño específico
    /// </summary>
    /// <param name="ownerId">ID del dueño</param>
    /// <returns>Lista de citas del dueño</returns>
    /// <response code="200">Lista de citas obtenida exitosamente</response>
    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByOwner(string ownerId)
    {
        var appointments = await _appointmentService.GetByUserIdAsync(ownerId);
        return Ok(appointments);
    }

    /// <summary>
    /// Obtiene las citas de una mascota específica
    /// </summary>
    /// <param name="petId">ID de la mascota</param>
    /// <returns>Lista de citas de la mascota</returns>
    /// <response code="200">Lista de citas obtenida exitosamente</response>
    [HttpGet("pet/{petId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPet(string petId)
    {
        var appointments = await _appointmentService.GetByPetIdAsync(petId);
        return Ok(appointments);
    }

    /// <summary>
    /// Actualiza una cita existente
    /// </summary>
    /// <param name="id">ID de la cita a actualizar</param>
    /// <param name="dto">Nuevos datos de la cita</param>
    /// <returns>Cita actualizada</returns>
    /// <response code="200">Cita actualizada exitosamente</response>
    /// <response code="400">Datos inválidos o conflicto de horarios</response>
    /// <response code="404">Cita no encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> Update(string id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAsync(id, dto);
            return Ok(appointment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una cita
    /// </summary>
    /// <param name="id">ID de la cita a eliminar</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Cita eliminada exitosamente</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(string id)
    {
        await _appointmentService.DeleteAsync(id);
        return NoContent();
    }
}