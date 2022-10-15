using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
  [Route("/api/c/platforms/{platformId}/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ICommandsRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandsRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
      Console.WriteLine($"--> Get commands for platform {platformId}.");

      if (!_repository.PlatformExists(platformId))
      {
        return NotFound();
      }

      var items = _repository.GetCommandsForPlatform(platformId);

      return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(items));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
      Console.WriteLine($"--> Get command {commandId} for platform {platformId}.");

      if (!_repository.PlatformExists(platformId))
      {
        return NotFound();
      }

      var item = _repository.GetCommand(platformId, commandId);

      if (item == null)
        return NotFound();

      return Ok(_mapper.Map<CommandReadDto>(item));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
      Console.WriteLine($"--> Create command {commandDto.CommandLine} for platform {platformId}.");

      if (!_repository.PlatformExists(platformId))
      {
        return NotFound();
      }

      var command = _mapper.Map<Command>(commandDto);

      _repository.CreateCommand(platformId, command);
      _repository.SaveChanges();

      var commandReadDto = _mapper.Map<CommandReadDto>(command);

      return CreatedAtRoute(
        nameof(GetCommandForPlatform),
        new { platformId, commandId = commandReadDto.Id },
        commandReadDto
      );
    }
  }
}