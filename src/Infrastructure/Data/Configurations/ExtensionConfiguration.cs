using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIExtensionsCenter.Infrastructure.Data.Configurations;
public class ExtensionConfiguration : IEntityTypeConfiguration<Extension>
{
    public void Configure(EntityTypeBuilder<Extension> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(150);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(150);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.HasIndex(e => e.Name)
            .IsUnique();

        // Relationships
        builder.HasMany(e => e.Licenses)
            .WithOne(l => l.Extension)
            .HasForeignKey(l => l.ExtensionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
