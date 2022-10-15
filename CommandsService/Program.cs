using Microsoft.EntityFrameworkCore;
using CommandsService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction()) {
    var connectionString = builder.Configuration.GetConnectionString("PlatformsDb");
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

builder.Services.AddScoped<ICommandsRepository, CommandsRepository>();

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
