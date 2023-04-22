namespace AspTemplate.Core.Dto.Auth;

/// <summary>
/// Данные ответа на авторизацию
/// </summary>
public class ResponseDto
{
    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Токен
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// Список ролей пользователя
    /// </summary>
    public List<int> RoleIds { get; set; }
}