using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformsService;

namespace CommandsService.Profiles
{
  public class CommandsProfile : Profile
  {
    public CommandsProfile()
    {
      // Source -> Target
      CreateMap<Platform, PlatformReadDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
      CreateMap<CommandCreateDto, Command>();
      CreateMap<Command, CommandReadDto>();

      CreateMap<PlatformPublishedDto, Platform>()
        .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Id, opt => opt.Ignore());
      
      CreateMap<GrpcPlatformModel, Platform>()
        .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
  }
}