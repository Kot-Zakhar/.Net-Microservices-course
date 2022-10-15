using CommandsService.Models;

namespace CommandsService.Data
{
  public interface ICommandsRepository
  {
    bool SaveChanges();

    IEnumerable<Command> GetAllCommands();
    void CreateCommand(int platformId, Command command);
    Command GetCommand(int platformId, int commandId);
    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform patform);
    bool PlatformExists(int platformId);
  }
}