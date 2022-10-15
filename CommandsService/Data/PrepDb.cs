using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
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


            if (context.Commands.Any()) {
                Console.WriteLine("--> We already have data.");
                return;
            }

            Console.WriteLine("--> Seed data");
            
            context.SaveChanges();
        }
    }
}