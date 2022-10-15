using PlatformsService.Models;

namespace PlatformsService.Data
{
  public interface IPlatformRepository
  {
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();

    Platform GetPlatform(int id);

    void CreatePlatform(Platform platform);
  }
}