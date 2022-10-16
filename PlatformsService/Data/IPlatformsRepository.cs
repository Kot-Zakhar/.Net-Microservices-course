using PlatformsService.Models;

namespace PlatformsService.Data
{
  public interface IPlatformsRepository
  {
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();

    Platform GetPlatform(int id);

    void CreatePlatform(Platform platform);
  }
}