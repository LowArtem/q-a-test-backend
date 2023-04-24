namespace AspTemplate.Core.Dto.Main;

public class QuestionDashboardResponseDto
{
    public string Title { get; set; }
    
    public int TypeId { get; set; }
    
    public List<OptionsStatsResponseDto> Options { get; set; }
}