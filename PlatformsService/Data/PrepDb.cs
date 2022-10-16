using Microsoft.EntityFrameworkCore;
using PlatformsService.Models;

namespace PlatformsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), env);
            }
        }

        private static void SeedData(AppDbContext context, IWebHostEnvironment env)
        {
            if (env.IsProduction()) {
                Console.WriteLine("--> Attempting to apply migrations...");
                try {
                    context.Database.Migrate();

                } catch (Exception e) {
                    Console.WriteLine($"--> Could not run migrations: {e.Message}");
                }
            }


            if (context.Platforms.Any()) {
                Console.WriteLine("--> We already have data.");
                return;
            }

            Console.WriteLine("--> Seed data");

            context.Platforms.AddRange(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" }
            );
            
            context.SaveChanges();
        }
    }
}