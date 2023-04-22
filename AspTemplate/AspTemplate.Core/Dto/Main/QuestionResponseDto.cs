namespace AspTemplate.Core.Dto.Main;

public class QuestionResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; }
    
    public int TypeId { get; set; }
    
    public List<OptionResponseDto> Options { get; set; }
}