using Microsoft.EntityFrameworkCore;

namespace MovieApi.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
    {
        
    }

}