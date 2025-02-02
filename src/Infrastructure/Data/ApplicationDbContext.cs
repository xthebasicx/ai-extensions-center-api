using System.Reflection;
using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIExtensionsCenter.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Extension> Extensions => Set<Extension>();
    public DbSet<License> Licenses => Set<License>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        Relationships(builder);
    }
    private void Relationships(ModelBuilder builder)
    {
        builder.Entity<Extension>()
            .HasMany(e => e.Licenses)
            .WithOne(e => e.Extension)
            .HasForeignKey(e => e.ExtensionId)
            .HasPrincipalKey(e => e.Id);
    }
}
