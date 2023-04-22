using System.Text.Json.Serialization;
using AspTemplate.Core.Model._Base;

namespace AspTemplate.Core.Model.Main;

public class Type : BaseEntity
{
    public string Name { get; set; }
    
    [JsonIgnore]
    public ICollection<Question> Questions { get; set; }
}