using AspTemplate.Api.Attributes;
using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Exceptions;
using AspTemplate.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AspTemplate.Api.Api.Main;

/// <summary>
/// Комнаты
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly RoomService _service;
    private readonly ILogger<RoomController> _logger;

    public RoomController(RoomService service, ILogger<RoomController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="requestDto">данные комнаты</param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200, "Создано")]
    [SwaggerResponse(500, "Ошибка")]
    public IActionResult CreateRoom(RoomRequestDto requestDto)
    {
        try
        {
            _service.CreateRoom(requestDto, User.Identity!.Name!);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return BadRequest("Пользователь не найден");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e}");
            return StatusCode(500, "Error: {e}");
        }
    }

    /// <summary>
    /// Получить комнату
    /// </summary>
    /// <param name="roomId">Id комнаты</param>
    /// <returns></returns>
    [HttpGet("{roomId}")]
    [SwaggerResponse(200, "Комната", typeof(RoomResponseDto))]
    [SwaggerResponse(500, "Ошибка")]
    [AllowAnonymous]
    public IActionResult GetRoom(int roomId)
    {
        try
        {
            return Ok(_service.GetRoom(roomId));
        }
        catch (EntityNotFoundException e)
        {
            return BadRequest("Комната не найдена");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e}");
            return StatusCode(500, "Error: {e}");
        }
    }

    /// <summary>
    /// Получить все комнаты данного пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Комнаты", typeof(List<RoomListResponseDto>))]
    [SwaggerResponse(500, "Ошибка")]
    public IActionResult GetRooms()
    {
        try
        {
            return Ok(_service.GetRooms(User.Identity!.Name!));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e}");
            return StatusCode(500, "Error: {e}");
        }
    }

    /// <summary>
    /// Удалить комнату
    /// </summary>
    /// <param name="roomId">Id комнату</param>
    /// <returns></returns>
    [HttpDelete("{roomId}")]
    [SwaggerResponse(200, "Удалено")]
    public IActionResult RemoveRoom(int roomId)
    {
        try
        {
            _service.RemoveRoom(roomId);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e}");
            return StatusCode(500, "Error: {e}");
        }
    }
}