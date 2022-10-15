using AutoMapper;
using PlatformsService.Dtos;
using PlatformsService.Models;

namespace PlatformsService.Profiles
{
  public class PlatformProfile : Profile
  {
    public PlatformProfile()
    {
      // Source -> Target
      CreateMap<Platform, PlatformReadDto>();
      CreateMap<PlatformCreateDto, Platform>();
      CreateMap<PlatformReadDto, PlatformPublishedDto>();
    }
  }
}