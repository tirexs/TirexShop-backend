using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;

namespace ProfileService.Infrastructure.Persistence;

public class ProfileDbContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; } = null!;

    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Profile>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasIndex(p => p.Id).IsUnique(); // важно для идемпотентности
        });
    }
}
