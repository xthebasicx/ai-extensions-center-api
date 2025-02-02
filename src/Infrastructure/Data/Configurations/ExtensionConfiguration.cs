using AIExtensionsCenter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIExtensionsCenter.Infrastructure.Data.Configurations;
public class ExtensionConfiguration : IEntityTypeConfiguration<Extension>
{
    public void Configure(EntityTypeBuilder<Extension> builder)
    {
        builder.Property(e => e.ExtensionName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(150);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(150);

        builder.HasIndex(e => e.ExtensionName)
            .IsUnique();

        // Relationship Configuration
        //builder.HasOne(e => e.User)
        //    .WithMany(u => u.Extensions)
        //    .HasForeignKey(e => e.UserId);
    }
}
