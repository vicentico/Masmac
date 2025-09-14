using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;

namespace VetUberApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Crea un nuevo usuario en el sistema
    /// </summary>
    /// <param name="dto">Datos del usuario a crear</param>
    /// <returns>El usuario creado con su ID asignado</returns>
    /// <response code="201">Usuario creado exitosamente</response>
    /// <response code="400">Email ya existe o datos inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _userService.CreateAsync(dto);
            return Created($"/api/users/{user.Id}", user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Datos del usuario</returns>
    /// <response code="200">Usuario encontrado</response>
    /// <response code="400">ID de usuario inválido</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "Usuario no encontrado" });

        return user;
    }

    /// <summary>
    /// Obtiene todos los usuarios del sistema
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    /// <response code="200">Lista de usuarios obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Actualiza los datos de un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="dto">Nuevos datos del usuario</param>
    /// <returns>Usuario actualizado</returns>
    /// <response code="200">Usuario actualizado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(string id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, dto);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un usuario del sistema (soft delete)
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Sin contenido</returns>
    /// <response code="204">Usuario eliminado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        var deleted = await _userService.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        
        return NoContent();
    }
}