using System.Text;
using System.Text.Json;
using PlatformsService.Dtos;

namespace PlatformsService.SyncDataServices.Http
{
  public class HttpCommandDataClient : ICommandDataClient
  {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
      _httpClient = httpClient;
      _configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
      var httpContext = new StringContent(
        JsonSerializer.Serialize(platform),
        Encoding.UTF8,
        "application/jon"
      );

      var response = await _httpClient.PostAsync($"{_configuration["CommandsService"]}/platforms/", httpContext);

      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine("--> Sync POST to CommandService was ok");
      }
      else
      {
        Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
      }
    }
  }
}