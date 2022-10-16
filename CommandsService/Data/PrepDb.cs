using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var client = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = client?.ReturnAllPlatforms() ?? new List<Platform>();

                SeedData(
                    serviceScope.ServiceProvider.GetService<AppDbContext>(),
                    serviceScope.ServiceProvider.GetService<ICommandsRepository>(),
                    platforms,
                    env
                );
            }
        }

        private static void SeedData(AppDbContext context, ICommandsRepository repo, IEnumerable<Platform> platforms, IWebHostEnvironment env)
        {
            if (env.IsProduction()) {
                Console.WriteLine("--> Attempting to apply migrations...");
                try {
                    context.Database.Migrate();
                } catch (Exception e) {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }
            }

            foreach (var plat in platforms) {
                if (!repo.PlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
            
            context.SaveChanges();
        }
    }
}