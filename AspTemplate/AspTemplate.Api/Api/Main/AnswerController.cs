using AspTemplate.Api.Attributes;
using AspTemplate.Core.Dto.Main;
using AspTemplate.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AspTemplate.Api.Api.Main;

/// <summary>
/// Ответы
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
public class AnswerController : ControllerBase
{
    private readonly AnswerService _service;

    public AnswerController(AnswerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Отправить ответы
    /// </summary>
    /// <param name="requestDtos"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200)]
    [SwaggerResponse(500)]
    public IActionResult CreateAnswers(List<AnswerRequestDto> requestDtos)
    {
        try
        {
            _service.SendAnswers(requestDtos);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e}");
        }
    }

    /// <summary>
    /// Получить ответы
    /// </summary>
    /// <param name="requestDtos"></param>
    /// <returns></returns>
    [HttpGet("{questionId}")]
    [SwaggerResponse(200, "успешно", typeof(List<AnswerResponseDto>))]
    [SwaggerResponse(500)]
    public IActionResult GetAnswers(int questionId)
    {
        try
        {
            return Ok(_service.GetAnswers(questionId));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Error: {e}");
        }
    }
}