using AspTemplate.Core.Dto;
using AspTemplate.Core.Dto.Auth;
using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Model.Auth;
using AspTemplate.Core.Model.Main;
using AutoMapper;

namespace AspTemplate.Api.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, ResponseDto>()
            .ForMember(
                dest => dest.RoleIds,
                opt =>
                    opt.MapFrom(u => u.UserRoles.Select(r => r.Id)));

        CreateMap<Option, OptionResponseDto>();
        CreateMap<Question, QuestionResponseDto>();
        CreateMap<AnswerRequestDto, Answer>();
        CreateMap<Answer, AnswerResponseDto>();
        CreateMap<RoomRequestDto, Room>();
        CreateMap<Room, RoomResponseDto>()
            .ForMember(dest => dest.QuestionsCount,
                opt => opt.MapFrom(r => r.Questions.Count));
    }
}