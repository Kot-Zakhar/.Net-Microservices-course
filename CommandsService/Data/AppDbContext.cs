using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Command> Commands { get; set; }
    public DbSet<Platform> Platforms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder
        .Entity<Platform>()
        .HasMany(p => p.Commands)
        .WithOne(p => p.Platform)
        .HasForeignKey(p => p.PlatformId);

      builder
        .Entity<Command>()
        .HasOne(c => c.Platform)
        .WithMany(c => c.Commands)
        .HasForeignKey(c => c.PlatformId);
    }
  }
}