using System.Text.Json.Serialization;
using System.Xml.Serialization;
using AspTemplate.Core.Model._Base;
using AspTemplate.Core.Model.Main;

namespace AspTemplate.Core.Model.Auth;

/// <summary>
/// Пользователь
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// ХЭШ пароля
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Role> UserRoles { get; set; }
    
    /// <summary>
    /// Комнаты пользователя
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Room> Rooms { get; set; } 
}