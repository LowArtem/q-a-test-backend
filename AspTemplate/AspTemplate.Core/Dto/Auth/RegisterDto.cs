using System.ComponentModel.DataAnnotations;

namespace AspTemplate.Core.Dto.Auth;

/// <summary>
/// Данные для регистрации пользователяы
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Почта
    /// </summary>
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public string Password { get; set; }
}