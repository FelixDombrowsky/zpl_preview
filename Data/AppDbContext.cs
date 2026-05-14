using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<LayoutTemplate> LayoutTemplates => Set<LayoutTemplate>();
  public DbSet<LocationLog> LocationLogs => Set<LocationLog>();
  public DbSet<Location> Locations => Set<Location>();
}
