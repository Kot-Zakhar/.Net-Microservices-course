using Microsoft.EntityFrameworkCore;
using PlatformsService.Models;

namespace PlatformsService.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<Platform> Platforms { get; set; }
  }
}