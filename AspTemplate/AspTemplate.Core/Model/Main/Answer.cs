using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AspTemplate.Core.Model._Base;

namespace AspTemplate.Core.Model.Main;

public class Answer : BaseEntity
{
    public int Value { get; set; }
    
    public int QuestionId { get; set; }
    
    [ForeignKey(nameof(QuestionId))]
    [JsonIgnore]
    public Question Question { get; set; }
}