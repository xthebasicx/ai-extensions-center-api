using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Extension> Extensions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
