﻿namespace AspTemplate.Core.Dto.Main;

public class RoomResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<int> QuestionIds { get; set; }
}