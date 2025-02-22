using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AIExtensionsCenter.Infrastructure.Data.Configurations
{
    public class APIKeyConfiguration : IEntityTypeConfiguration<APIKey>
    {
        public void Configure(EntityTypeBuilder<APIKey> builder)
        {
            builder.Property(a => a.Key)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<APIKey>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
