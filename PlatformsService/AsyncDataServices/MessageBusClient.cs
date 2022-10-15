using System.Text;
using System.Text.Json;
using PlatformsService.Dtos;
using RabbitMQ.Client;

namespace PlatformsService.AsyncDataServices
{
  public class MessageBusClient : IMessageBustClient, IDisposable
  {
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private static string exchange = "trigger";

    public MessageBusClient(IConfiguration configuration)
    {
      _configuration = configuration;

      var factory = new ConnectionFactory() {
        HostName = _configuration["RabbitMQHost"],
        Port = int.Parse(_configuration["RabbitMQPort"])
      };

      try
      {
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        Console.WriteLine("--> Connected to Message bus");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
      }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
      var message = JsonSerializer.Serialize(platformPublishedDto);

      if (_connection.IsOpen)
      {
        Console.WriteLine("--> Sending message to MB...");
        SendMessage(message);
      } else {
        Console.WriteLine("->> MB connection closed.");
      }

      // TODO: Can implement retry strategy
    }

    private void SendMessage(string message)
    {
      var body = Encoding.UTF8.GetBytes(message);

      _channel.BasicPublish(
        exchange,
        routingKey: "",
        basicProperties: null,
        body: body
      );

      Console.WriteLine("--> Message is sent.");
    }

    public void Dispose()
    {
      Console.WriteLine("MessageBus disposed");
      if (_channel.IsOpen)
      {
        _channel.Close();
        _connection.Close();
      }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
      Console.WriteLine("--> RabbitMQ connection shutdown");
    }
  }
}