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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>().HasKey(f => f.Id);
        modelBuilder.Entity<Flight>().Property(f=>f.Destination).HasColumnType("varchar(256)");
        modelBuilder.Entity<Flight>().Property(f => f.Origin).HasColumnType("varchar(256)");
        
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().Property(u=>u.Username).HasColumnType("varchar(256)");
        modelBuilder.Entity<User>().Property(u=>u.Password).HasColumnType("varchar(256)");
        
        modelBuilder.Entity<Role>().HasKey(r => r.Id);
        modelBuilder.Entity<Role>().HasIndex(r => r.Code).IsUnique();
        modelBuilder.Entity<Role>().Property(r=>r.Code).HasColumnType("varchar(256)");
        
    }

}