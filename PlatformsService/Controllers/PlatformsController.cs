using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformsService.Data;
using PlatformsService.Dtos;
using PlatformsService.Models;
using PlatformsService.SyncDataServices.Http;

namespace PlatformsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;


        public PlatformsController(
            IPlatformRepository platformRepo,
            IMapper mapper,
            ICommandDataClient commandDataClient
        )
        {
            _repository = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms");

            var items = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(items));
        }

        [HttpGet]
        [Route("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var item = _repository.GetPlatform(id);

            if (item is not null)
                return Ok(_mapper.Map<PlatformReadDto>(item));

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody]PlatformCreateDto platformDto)
        {
            var item = _mapper.Map<Platform>(platformDto);
            _repository.CreatePlatform(item);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(item);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send platform to commandService: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
        }
    }
}