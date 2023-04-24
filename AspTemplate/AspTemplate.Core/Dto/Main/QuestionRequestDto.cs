namespace AspTemplate.Core.Dto.Main;

public class QuestionRequestDto
{
    public int RoomId { get; set; }
    
    public string Title { get; set; }
    
    public List<OptionRequestDto> Options { get; set; }
}