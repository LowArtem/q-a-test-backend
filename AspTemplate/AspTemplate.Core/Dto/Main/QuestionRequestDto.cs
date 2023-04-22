namespace AspTemplate.Core.Dto.Main;

public class QuestionRequestDto
{
    public string Title { get; set; }
    
    public List<OptionRequestDto> Options { get; set; }
}