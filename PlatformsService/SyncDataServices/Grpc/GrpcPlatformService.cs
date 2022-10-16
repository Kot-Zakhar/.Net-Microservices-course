using AutoMapper;
using Grpc.Core;
using PlatformsService.Data;

namespace PlatformsService.SyncDataServices.Grpc
{
  public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
  {
    private readonly IPlatformsRepository _repository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformsRepository repo, IMapper mapper)
    {
      _repository = repo;
      _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
      var response = new PlatformResponse();
      var platforms = _repository.GetAllPlatforms();

      foreach(var plat in platforms)
      {
        response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
      }

      return Task.FromResult(response);
    }
  }
}