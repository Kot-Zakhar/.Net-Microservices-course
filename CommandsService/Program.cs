using Microsoft.EntityFrameworkCore;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.AsyncDataServices;
using CommandsService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction()) {
    var connectionStringFormat = builder.Configuration.GetConnectionString("CommandsDb");
    var server = Environment.GetEnvironmentVariable("DB_SERVER");
    var user = Environment.GetEnvironmentVariable("DB_USER");
    var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var connectionString = String.Format(connectionStringFormat, server, user, pass);
    
    Console.WriteLine($"--> Using SqlServer db: {connectionString}");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
} else {
    Console.WriteLine("--> Using in memory db");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<ICommandsRepository, CommandsRepository>();
if (builder.Environment.IsProduction()) {
    builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.PrepPopulation(app.Environment);

app.Run();
