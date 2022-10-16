using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
  public class EventProcessor : IEventProcessor
  {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
      IServiceScopeFactory scopeFactory,
      IMapper mapper
    ) {
      _scopeFactory = scopeFactory;
      _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
      var eventType = DetermineEvent(message);

      switch (eventType)
      {
        case EventType.PlatformPublished:
          AddPlatform(message);
          break;
        default:
          break;
      }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
      Console.WriteLine($"--> Determining Event");
      var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

      switch (eventType.Event)
      {
        case "Platform_Published":
          Console.WriteLine("--> Platform Published Event Detected");
          return EventType.PlatformPublished;
        default:
          return EventType.Undetermined;
      }
    }

    private void AddPlatform(string platformMessage)
    {
      using var scope = _scopeFactory.CreateScope();

      var repo = scope.ServiceProvider.GetRequiredService<ICommandsRepository>();
      
      var publishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformMessage);

      try
      {
        var platform = _mapper.Map<Platform>(publishedDto);
        if (!repo.PlatformExists(platform.ExternalId))
        {
          Console.WriteLine($"Trying to save platform: {platform}");
          repo.CreatePlatform(platform);
          repo.SaveChanges();
        }
        else
        {
          Console.WriteLine("Platform already exists.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Error adding platform: {ex.Message}");
      }
    }
  }
}