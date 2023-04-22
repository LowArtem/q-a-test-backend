using System.Text.Json.Serialization;
using AspTemplate.Core.Model._Base;

namespace AspTemplate.Core.Model.Main;

public class Room : BaseEntity
{
    [JsonIgnore]
    public ICollection<Question> Questions { get; set; }
}