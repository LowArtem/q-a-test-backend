using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Exceptions;
using AspTemplate.Core.Model.Main;
using AspTemplate.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Data.Services;

public class AnswerService
{
    private readonly IEfCoreRepository<Answer> _answerRepository;
    private readonly IEfCoreRepository<Room> _roomRepository;
    private readonly IMapper _mapper;

    public AnswerService(IEfCoreRepository<Answer> answerRepository, IMapper mapper,
        IEfCoreRepository<Room> roomRepository)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public void SendAnswers(List<AnswerRequestDto> answers)
    {
        if (answers.Count == 0)
            return;

        _answerRepository.AddRange(
            answers.Select(a => _mapper.Map<Answer>(a))
        );
    }

    public QuestionDashboardResponseDto GetAnswers(int roomId, int questionNumber)
    {
        var room = _roomRepository.GetListQuery()
            .Include(r => r.Questions)
            .ThenInclude(q => q.Options)
            .SingleOrDefault(r => r.Id == roomId);

        if (room == null)
        {
            throw new EntityNotFoundException(typeof(Room));
        }

        var question = room.Questions.ToList().OrderBy(x => x.Id).ElementAt(questionNumber - 1);
        var answers = _answerRepository.GetListQuery()
            .Where(a => a.QuestionId == question.Id)
            .ToList();

        var options = question.Options.Select(o => new OptionsStatsResponseDto
        {
            Description = o.Description,
            Value = o.Value,
            AnswersCount = answers.Count(a => a.Value == o.Value)
        });
        
        return new QuestionDashboardResponseDto
        {
            Title = question.Title,
            TypeId = question.TypeId,
            Options = options.ToList()
        };
    }
}