using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("alert");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Description).HasColumnName("description");
        builder.Property(a => a.DateTime).HasColumnName("date_time").IsRequired();
        builder.Property(a => a.MaxPlayers).HasColumnName("max_players").IsRequired();
        builder.Property(a => a.Ball).HasColumnName("ball").HasDefaultValue(false);
        builder.Property(a => a.Net).HasColumnName("net").HasDefaultValue(false);
        builder.Property(a => a.ZoneId).HasColumnName("zone_id").IsRequired();
        builder.Property(a => a.LevelId).HasColumnName("level_id").IsRequired();
        builder.Property(a => a.CreatorId).HasColumnName("creator_id").IsRequired();

        builder.HasOne(a => a.Zone)
            .WithMany(z => z.Alerts)
            .HasForeignKey(a => a.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Level)
            .WithMany(l => l.Alerts)
            .HasForeignKey(a => a.LevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Creator)
            .WithMany(u => u.CreatedAlerts)
            .HasForeignKey(a => a.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}