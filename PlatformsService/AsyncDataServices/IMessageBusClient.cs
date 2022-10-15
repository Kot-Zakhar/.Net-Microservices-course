using PlatformsService.Dtos;

namespace PlatformsService.AsyncDataServices
{
  public interface IMessageBustClient
  {
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
  }
}