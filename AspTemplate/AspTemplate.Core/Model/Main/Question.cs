using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AspTemplate.Core.Model._Base;

namespace AspTemplate.Core.Model.Main;

public class Question : BaseEntity
{
    public string Title { get; set; }
    
    public int TypeId { get; set; }
    
    [ForeignKey(nameof(TypeId))]
    [JsonIgnore]
    public Type Type { get; set; }
    
    public int RoomId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    [JsonIgnore]
    public Room Room { get; set; }
    
    [JsonIgnore]
    public ICollection<Answer> Answers { get; set; }
    
    [JsonIgnore]
    public ICollection<Option> Options { get; set; }
}