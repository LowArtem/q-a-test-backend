using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Model.Main;
using AspTemplate.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Data.Services;

public class AnswerService
{
    private readonly IEfCoreRepository<Answer> _answerRepository;
    private readonly IMapper _mapper;

    public AnswerService(IEfCoreRepository<Answer> answerRepository, IMapper mapper)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
    }

    public void SendAnswers(List<AnswerRequestDto> answers)
    {
        if (answers.Count == 0)
            return;

        _answerRepository.AddRange(
            answers.Select(a => _mapper.Map<Answer>(a))
        );
    }

    public List<AnswerResponseDto> GetAnswers(int questionId)
    {
        return _answerRepository.GetListQuery()
            .Where(a => a.QuestionId == questionId)
            .Select(a => _mapper.Map<AnswerResponseDto>(a))
            .ToList();
    }
}