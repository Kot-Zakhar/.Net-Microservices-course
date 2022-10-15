using CommandsService.Models;

namespace CommandsService.Data
{
  public class CommandsRepository : ICommandsRepository
  {
    private readonly AppDbContext _context;

    public CommandsRepository(AppDbContext context)
    {
      _context = context;
    }

    public IEnumerable<Command> GetAllCommands()
    {
      return _context.Commands.ToList();
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
      return _context.Commands
        .Where(c => c.PlatformId == platformId)
        .OrderBy(c => c.PlatformId)
        .ToList();
    }

    public void CreateCommand(int platformId, Command command)
    {
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      command.PlatformId = platformId;
      
      _context.Commands.Add(command);
    }

    public Command GetCommand(int platformId, int commandId)
    {
      return _context.Commands.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
    }


    public void CreatePlatform(Platform platform)
    {
      if (platform == null)
      {
        throw new ArgumentNullException(nameof(platform));
      }

      _context.Platforms.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
      return _context.Platforms.ToList();
    }

    public bool PlatformExists(int platformId)
    {
      return _context.Platforms.Any(p => p.Id == platformId);
    }


    public bool SaveChanges()
    {
      return _context.SaveChanges() >= 0;
    }
  }
}