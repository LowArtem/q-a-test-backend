using AspTemplate.Core.Dto;
using AspTemplate.Core.Dto.Auth;
using AspTemplate.Core.Model.Auth;
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
    }
}