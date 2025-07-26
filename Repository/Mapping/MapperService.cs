using Application.Interfaces.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Response;
using Profile = AutoMapper.Profile;

namespace Repository.Mapping;

public class MapperService : Profile
{
    public MapperService()
    {
        CreateMap<Runner, RunnerDto>();
        
        CreateMap<RunnerDto, Runner>();
        
        CreateMap<Runner, CreateAccountResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId));
        
        CreateMap<AccountCreateRequest, Runner>()
            .ForMember(dest => dest.RunnerId, opt => opt.Ignore())
            .ForMember(dest => dest.Profile, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }
}