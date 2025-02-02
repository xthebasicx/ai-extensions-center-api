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

        builder.HasIndex(l => l.LicenseKey)
            .IsUnique();

        // Relationships
        builder.HasOne(l => l.Extension)
            .WithMany(e => e.Licenses)
            .HasForeignKey(l => l.ExtensionId)
            .OnDelete(DeleteBehavior.Cascade);

        //builder.HasOne(l => l.User)
        //    .WithMany()
        //    .HasForeignKey(l => l.UserId);
    }
}

