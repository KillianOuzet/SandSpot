using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class JoinConfiguration : IEntityTypeConfiguration<Join>
{
    public void Configure(EntityTypeBuilder<Join> builder)
    {
        builder.ToTable("join");

        // Clé primaire composée
        builder.HasKey(j => new { j.UserId, j.AlertId });

        builder.Property(j => j.UserId).HasColumnName("user_id");
        builder.Property(j => j.AlertId).HasColumnName("alert_id");

        builder.HasOne(j => j.User)
            .WithMany(u => u.JoinedAlerts)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(j => j.Alert)
            .WithMany(a => a.Participants)
            .HasForeignKey(j => j.AlertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}