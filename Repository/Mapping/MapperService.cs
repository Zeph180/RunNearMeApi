using Application.Interfaces.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Repository.Mapping;

public class MapperService : Profile
{
    public MapperService()
    {
        CreateMap<Runner, RunnerDto>();
        CreateMap<RunnerDto, Runner>();
    }
}