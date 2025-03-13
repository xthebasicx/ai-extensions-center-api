using AIExtensionsCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIExtensionsCenter.Infrastructure.Data.Configurations;
public class LicenseConfiguration : IEntityTypeConfiguration<License>
{
    public void Configure(EntityTypeBuilder<License> builder)
    {
        builder.Property(l => l.LicenseKey)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(l => l.ExpirationDate)
            .IsRequired();

        builder.Property(l => l.ActivatedByUserEmail)
            .HasMaxLength(50);

        builder.Property(l => l.ActivatedMachineId)
            .HasMaxLength(50);

        builder.HasIndex(l => l.LicenseKey)
            .IsUnique();

        // Relationships
        builder.HasOne(l => l.Extension)
            .WithMany(e => e.Licenses)
            .HasForeignKey(l => l.ExtensionId)
            .HasPrincipalKey(e => e.Id);
    }
}
