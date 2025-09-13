using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    /// <summary>
    /// Crea una nueva reseña
    /// </summary>
    /// <param name="dto">Datos de la reseña a crear</param>
    /// <returns>La reseña creada</returns>
    /// <response code="201">Reseña creada exitosamente</response>
    /// <response code="400">Datos inválidos o el usuario ya ha realizado una reseña para esta cita</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto)
    {
        try
        {
            var review = await _reviewService.CreateAsync(dto);
            return Created($"/api/reviews/{review.Id}", review);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una reseña por su ID
    /// </summary>
    /// <param name="id">ID de la reseña</param>
    /// <returns>Datos de la reseña</returns>
    /// <response code="200">Reseña encontrada</response>
    /// <response code="404">Reseña no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> GetById(string id)
    {
        var review = await _reviewService.GetByIdAsync(id);
        if (review == null)
            return NotFound();

        return review;
    }

    /// <summary>
    /// Obtiene todas las reseñas
    /// </summary>
    /// <returns>Lista de reseñas</returns>
    /// <response code="200">Lista de reseñas obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
    {
        var reviews = await _reviewService.GetAllAsync();
        return Ok(reviews);
    }

    /// <summary>
    /// Obtiene todas las reseñas de un veterinario específico
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Lista de reseñas del veterinario</returns>
    /// <response code="200">Lista de reseñas obtenida exitosamente</response>
    [HttpGet("veterinarian/{veterinarianId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByVeterinarian(string veterinarianId)
    {
        var reviews = await _reviewService.GetByVeterinarianIdAsync(veterinarianId);
        return Ok(reviews);
    }

    /// <summary>
    /// Obtiene todas las reseñas realizadas por un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de reseñas del usuario</returns>
    /// <response code="200">Lista de reseñas obtenida exitosamente</response>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByUser(string userId)
    {
        var reviews = await _reviewService.GetByUserIdAsync(userId);
        return Ok(reviews);
    }

    /// <summary>
    /// Obtiene el promedio de calificaciones de un veterinario
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Promedio de calificaciones</returns>
    /// <response code="200">Promedio calculado exitosamente</response>
    [HttpGet("veterinarian/{veterinarianId}/average-rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetVeterinarianAverageRating(string veterinarianId)
    {
        var average = await _reviewService.GetVeterinarianAverageRatingAsync(veterinarianId);
        return Ok(average);
    }

    /// <summary>
    /// Actualiza una reseña existente
    /// </summary>
    /// <param name="id">ID de la reseña a actualizar</param>
    /// <param name="dto">Nuevos datos de la reseña</param>
    /// <returns>Reseña actualizada</returns>
    /// <response code="200">Reseña actualizada exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Reseña no encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> Update(string id, [FromBody] UpdateReviewDto dto)
    {
        try
        {
            var review = await _reviewService.UpdateAsync(id, dto);
            return Ok(review);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una reseña
    /// </summary>
    /// <param name="id">ID de la reseña a eliminar</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Reseña eliminada exitosamente</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(string id)
    {
        await _reviewService.DeleteAsync(id);
        return NoContent();
    }
}