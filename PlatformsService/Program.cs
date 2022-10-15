using Microsoft.EntityFrameworkCore;
using PlatformsService.SyncDataServices.Http;
using PlatformsService.Data;
using PlatformsService.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction()) {
    var connectionStringFormat = builder.Configuration.GetConnectionString("PlatformsDb");
    var server = Environment.GetEnvironmentVariable("DB_SERVER");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var connectionString = String.Format(connectionStringFormat, server, user, pass);

    Console.WriteLine($"--> Using SqlServer db: {connectionString}");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

} else {
    Console.WriteLine("--> Using in memory db");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

    Environment.SetEnvironmentVariable("COMMANDS_SERVICE_ADDRESS", builder.Configuration.GetValue<string>("CommandsService"));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBustClient, MessageBusClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else {
}

Console.WriteLine("--> Commands service address: " + app.Configuration.GetValue<string>("CommandsService"));

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.PrepPopulation(app.Environment);

app.Run();
