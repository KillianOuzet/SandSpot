using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("zone");
        builder.HasKey(z => z.Id);

        builder.Property(z => z.Id).HasColumnName("id");
        builder.Property(z => z.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(z => z.Latitude).HasColumnName("latitude").IsRequired();
        builder.Property(z => z.Longitude).HasColumnName("longitude").IsRequired();
        builder.Property(z => z.Address).HasColumnName("address").HasMaxLength(255).IsRequired();
        builder.Property(z => z.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        builder.Property(z => z.PostalCode).HasColumnName("postal_code").HasMaxLength(10).IsRequired();
    }
}