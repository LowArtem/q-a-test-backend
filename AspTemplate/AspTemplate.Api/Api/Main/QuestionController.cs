using AspTemplate.Api.Attributes;
using AspTemplate.Core.Dto.Main;
using AspTemplate.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AspTemplate.Api.Api.Main;

/// <summary>
/// Вопросы
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
[Authorize]
public class QuestionController : ControllerBase
{
    private readonly QuestionService _service;

    public QuestionController(QuestionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Создать вопрос
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200)]
    [SwaggerResponse(500)]
    public IActionResult CreateQuestion(QuestionRequestDto dto)
    {
        try
        {
            _service.CreateQuestion(dto);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e}");
        }
    }

    /// <summary>
    /// Получить все вопросы комнаты
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    [HttpGet("room/{roomId}")]
    [SwaggerResponse(200, "Успешно", typeof(List<QuestionResponseDto>))]
    [SwaggerResponse(500)]
    public IActionResult GetQuestionByRoom([FromRoute] int roomId)
    {
        try
        {
            return Ok(_service.GetQuestionsByRoom(roomId));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e}");
        }
    }

    /// <summary>
    /// Получить вопрос по id
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpGet("id/{questionId}")]
    [SwaggerResponse(200, "Успешно", typeof(QuestionResponseDto))]
    [SwaggerResponse(500)]
    public IActionResult GetQuestionById([FromRoute] int questionId)
    {
        try
        {
            return Ok(_service.GetQuestion(questionId));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e}");
        }
    }
}