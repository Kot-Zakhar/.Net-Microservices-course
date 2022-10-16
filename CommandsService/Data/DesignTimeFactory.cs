using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommandsService.Data
{
  public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
  {
    public AppDbContext CreateDbContext(string[] args)
    {
      string path = Directory.GetCurrentDirectory();

      IConfigurationBuilder builder =
          new ConfigurationBuilder()
              .SetBasePath(path)
              .AddJsonFile("appsettings.Production.json");

      IConfigurationRoot config = builder.Build();

      string connectionString = config.GetConnectionString("CommandsDb");
      
      var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
      optionsBuilder.UseSqlServer(connectionString);

      return new AppDbContext(optionsBuilder.Options);
    }
  }
}