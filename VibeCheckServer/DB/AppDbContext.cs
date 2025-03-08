namespace VibeCheckServer.DB;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Response> Responses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}
