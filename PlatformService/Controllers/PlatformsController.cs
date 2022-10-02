using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        public readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository platformRepo, IMapper mapper)
        {
            _repository = platformRepo;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> CreatePlatform([FromBody]PlatformCreateDto platformDto)
        {
            var item = _mapper.Map<Platform>(platformDto);
            _repository.CreatePlatform(item);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(item);

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
        }
    }
}