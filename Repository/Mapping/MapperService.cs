using Application.Interfaces;
using Application.Interfaces.Dtos;
using Application.Interfaces.Dtos.Challenge;
using Application.Interfaces.Dtos.Post;
using Application.Interfaces.Dtos.Run;
using Application.Models.Request.Authentication;
using Application.Models.Request.Challenge;
using Application.Models.Request.Posts;
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
        CreateMap<Domain.Entities.Profile ,Person>();

        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes != null ? src.Likes.Count : 0))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
        
        CreateMap<Comment, CommentDto>();
        
        CreateMap<CommentRequest, Comment>();
        
        CreateMap<UpdatePostRequest, Post>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Caption));
        
        CreateMap<Post, CreatePostResponse>()
            .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Message));
        
        CreateMap<CreatePostRequest, Post>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Caption))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId));
        
        CreateMap<UpdateChallengeRequest, Challenge>();
        
        CreateMap<Challenge, ChallengeDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Target, opt => opt.MapFrom(src => src.Target))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.EndsAt, opt => opt.MapFrom(src => src.EndsAt));
        
        CreateMap<CreateChallengeRequest, Challenge>()
            .ForMember(dest => dest.ChallengeId, opt => opt.Ignore())
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Target, opt => opt.MapFrom(src => src.Target))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.EndsAt, opt => opt.MapFrom(src => src.EndsAt));
            
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

        CreateMap<Friend, FriendRequestResponse>()
            .ForMember(dest => dest.FriendRequestId, opt => opt.MapFrom(src => src.FriendId))
            .ForMember(dest => dest.RequestStatus, opt => opt.MapFrom(src => src.Status));
        
        CreateMap<Domain.Entities.Profile, Friend>()
            .ForMember(dest => dest.RequestTo, opt => opt.MapFrom(src => src.RunnerId));

        CreateMap<RunDto, Run>()
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.RunId, opt => opt.MapFrom(src => src.RunId))
            .ForMember(dest => dest.AverageHeartRate, opt => opt.MapFrom(src => src.AverageHeartRate))
            .ForMember(dest => dest.DistanceInMeters, opt => opt.MapFrom(src => src.DistanceInMeters))
            .ForMember(dest => dest.AveragePace, opt => opt.MapFrom(src => src.AveragePace))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.DurationInSeconds))
            .ForMember(dest => dest.MaxPace, opt => opt.MapFrom(src => src.MaxPace))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RoutePoints, opt => opt.MapFrom(src => src.RoutePoints));

        CreateMap<RoutePointDto, RunRoutePoint>()
            .ForMember(dest => dest.RunRoutePointId, opt => opt.Ignore())
            .ForMember(dest => dest.RunId, opt => opt.Ignore())
            .ForMember(dest => dest.Run, opt => opt.Ignore());
        
        CreateMap<Run, RunDto>()
            .ForMember(dest => dest.RunnerId, opt => opt.MapFrom(src => src.RunnerId))
            .ForMember(dest => dest.RunId, opt => opt.MapFrom(src => src.RunId))
            .ForMember(dest => dest.AverageHeartRate, opt => opt.MapFrom(src => src.AverageHeartRate))
            .ForMember(dest => dest.DistanceInMeters, opt => opt.MapFrom(src => src.DistanceInMeters))
            .ForMember(dest => dest.AveragePace, opt => opt.MapFrom(src => src.AveragePace))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.DurationInSeconds))
            .ForMember(dest => dest.MaxPace, opt => opt.MapFrom(src => src.MaxPace))
            .ForMember(dest => dest.RoutePoints, opt => opt.MapFrom(src => src.RoutePoints));

        CreateMap<RunRoutePoint, RoutePointDto>()
            .ForMember(dest => dest.Altitude, opt => opt.MapFrom(src => src.Altitude))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude,
                opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Timestamp,
                opt => opt.MapFrom(src => src.Timestamp));
    }
}