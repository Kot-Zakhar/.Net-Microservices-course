using Microsoft.AspNetCore.Mvc;
using CommandsService.Data;
using AutoMapper;
using CommandsService.Dtos;

namespace CommandsService.Controllers
{
  [Route("api/c/[controller]")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly ICommandsRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandsRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
      var items = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(items));
    }

    [HttpPost]
    public ActionResult TestInboundConnection() {
      Console.WriteLine("--> Inbound POST # Command Service");

      return Ok("Inbound test ok from Platforms Controller");
    }
  }
}