using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Model.Main;
using AspTemplate.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Data.Services;

public class QuestionService
{
    private readonly IEfCoreRepository<Question> _questionRepository;
    private readonly IEfCoreRepository<Option> _optionRepository;
    private readonly IMapper _mapper;

    public QuestionService(IEfCoreRepository<Question> questionRepository, IEfCoreRepository<Option> optionRepository,
        IMapper mapper)
    {
        _questionRepository = questionRepository;
        _optionRepository = optionRepository;
        _mapper = mapper;
    }

    public void CreateQuestion(QuestionRequestDto dto)
    {
        var question = new Question
        {
            Title = dto.Title,
            RoomId = 1,
            TypeId = 1
        };

        _questionRepository.Add(question);
        _questionRepository.SaveChanges();

        var options = dto.Options.Select((o, i) => new Option
        {
            Description = o.Description,
            QuestionId = question.Id,
            Value = i
        });

        _optionRepository.AddRange(options);
        _optionRepository.SaveChanges();
    }

    public QuestionResponseDto? GetQuestion(int id)
    {
        var result = _questionRepository.GetListQuery()
            .Include(q => q.Options)
            .FirstOrDefault(q => q.Id == id);

        return result == null ? null : _mapper.Map<QuestionResponseDto>(result);
    }
    
    public List<QuestionResponseDto> GetQuestionsByRoom(int roomId)
    {
        return _questionRepository.GetListQuery()
            .Include(q => q.Options)
            .Where(q => q.RoomId == roomId)
            .Select(q => _mapper.Map<QuestionResponseDto>(q))
            .ToList();
    }
}