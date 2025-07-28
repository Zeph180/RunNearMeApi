using Application.Interfaces.Dtos;
using Application.Models.Request.Authentication;
using Application.Models.Response.People;
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

        CreateMap<CompleteProfileReq, Domain.Entities.Profile>()
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

        CreateMap<Domain.Entities.Profile, GetPersonResponse>()
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.NickName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

        CreateMap<Domain.Entities.Profile, Friend>()
            .ForMember(dest => dest.FriendId, opt => opt.Ignore())
            .ForMember(dest => dest.RequestTo, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Profiles, opt => opt.Ignore());

        CreateMap<Friend, FriendRequestResponse>()
            .ForMember(dest => dest.RequestId, opt => opt.Ignore())
            .ForMember(dest => dest.RequestStatus, opt => opt.MapFrom(src => src.Status));
    }
}