using Microsoft.EntityFrameworkCore;

namespace TechSpec.Models;

public sealed class Context : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

}