using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class PetsController : ControllerBase
{
    private readonly PetService _petService;

    public PetsController(PetService petService)
    {
        _petService = petService;
    }

    /// <summary>
    /// Registra una nueva mascota en el sistema
    /// </summary>
    /// <param name="dto">Datos de la mascota a registrar</param>
    /// <returns>La mascota registrada con su ID asignado</returns>
    /// <response code="201">Mascota registrada exitosamente</response>
    /// <response code="400">Datos inválidos o dueño no encontrado</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PetDto>> Create([FromBody] CreatePetDto dto)
    {
        try
        {
            var pet = await _petService.CreateAsync(dto);
            return Created($"/api/pets/{pet.Id}", pet);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una mascota por su ID
    /// </summary>
    /// <param name="id">ID de la mascota</param>
    /// <returns>Datos de la mascota</returns>
    /// <response code="200">Mascota encontrada</response>
    /// <response code="404">Mascota no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetDto>> GetById(string id)
    {
        var pet = await _petService.GetByIdAsync(id);
        if (pet == null)
            return NotFound();

        return pet;
    }

    /// <summary>
    /// Obtiene todas las mascotas registradas en el sistema
    /// </summary>
    /// <returns>Lista de mascotas</returns>
    /// <response code="200">Lista de mascotas obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PetDto>>> GetAll()
    {
        var pets = await _petService.GetAllAsync();
        return Ok(pets);
    }

    /// <summary>
    /// Obtiene todas las mascotas de un dueño específico
    /// </summary>
    /// <param name="ownerId">ID del dueño</param>
    /// <returns>Lista de mascotas del dueño</returns>
    /// <response code="200">Lista de mascotas obtenida exitosamente</response>
    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PetDto>>> GetByOwner(string ownerId)
    {
        var pets = await _petService.GetByUserIdAsync(ownerId);
        return Ok(pets);
    }

    /// <summary>
    /// Actualiza los datos de una mascota existente
    /// </summary>
    /// <param name="id">ID de la mascota a actualizar</param>
    /// <param name="dto">Nuevos datos de la mascota</param>
    /// <returns>Mascota actualizada</returns>
    /// <response code="200">Mascota actualizada exitosamente</response>
    /// <response code="404">Mascota no encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetDto>> Update(string id, [FromBody] UpdatePetDto dto)
    {
        try
        {
            var pet = await _petService.UpdateAsync(id, dto);
            return Ok(pet);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una mascota del sistema (soft delete)
    /// </summary>
    /// <param name="id">ID de la mascota a eliminar</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Mascota eliminada exitosamente</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(string id)
    {
        await _petService.DeleteAsync(id);
        return NoContent();
    }
}