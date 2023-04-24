using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AspTemplate.Core.Model._Base;
using AspTemplate.Core.Model.Auth;

namespace AspTemplate.Core.Model.Main;

public class Room : BaseEntity
{
    public string Name { get; set; }
    
    [JsonIgnore]
    public ICollection<Question> Questions { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    [JsonIgnore]
    public User User { get; set; }
}