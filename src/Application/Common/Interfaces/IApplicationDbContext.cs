using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Extension> Extensions { get; }
    public DbSet<License> Licenses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
