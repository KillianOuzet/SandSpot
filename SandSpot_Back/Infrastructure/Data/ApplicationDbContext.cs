using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mappings spécifiques PostgreSQL & Contraintes

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");
            entity.HasKey(e => e.IdRole);
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.IdUser);
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Mail).HasColumnName("mail").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).HasColumnName("password").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
            entity.Property(e => e.IdRole).HasColumnName("id_role");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("zone");
            entity.HasKey(e => e.IdZone);
            entity.Property(e => e.IdZone).HasColumnName("id_zone");
            entity.Property(e => e.Coordinates).HasColumnName("coordinates").HasMaxLength(255);
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(255);
            entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Zones)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.ToTable("alert");
            entity.HasKey(e => e.IdAlert);
            entity.Property(e => e.IdAlert).HasColumnName("id_alert");
            entity.Property(e => e.DateAlert).HasColumnName("date_alert");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Ball).HasColumnName("ball");
            entity.Property(e => e.Net).HasColumnName("net");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdZone).HasColumnName("id_zone");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Alerts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Zone)
                .WithMany(p => p.Alerts)
                .HasForeignKey(d => d.IdZone)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("message");
            entity.HasKey(e => e.IdMessage);
            entity.Property(e => e.IdMessage).HasColumnName("id_message");
            entity.Property(e => e.DateMessage).HasColumnName("date_message");
            entity.Property(e => e.Content).HasColumnName("content").IsRequired();
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdAlert).HasColumnName("id_alert");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Alert)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdAlert)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");
            entity.HasKey(e => e.IdReview);
            entity.Property(e => e.IdReview).HasColumnName("id_review");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.DateReview).HasColumnName("date_review");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdZone).HasColumnName("id_zone");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Zone)
                .WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdZone)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.ToTable("like");
            entity.HasKey(e => e.IdLike);
            entity.Property(e => e.IdLike).HasColumnName("id_like");
            entity.Property(e => e.HasLiked).HasColumnName("has_liked");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdAlert).HasColumnName("id_alert");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Likes)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Alert)
                .WithMany(p => p.Likes)
                .HasForeignKey(d => d.IdAlert)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}