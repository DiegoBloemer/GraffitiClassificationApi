using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Models;

namespace GraffitiClassificationApi.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Gang> Gangs { get; set; }
    public DbSet<Graffiti> Graffitis { get; set; }
    public DbSet<GraffitiLocation> GraffitiLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Table: gangs (entity Gang) ---
        modelBuilder.Entity<Gang>(e =>
        {
            e.ToTable("gangs");
            e.Property(g => g.Id).HasColumnName("id");
            e.Property(g => g.Name).HasColumnName("name");
            e.Property(g => g.Acronym).HasColumnName("acronym");
            e.Property(g => g.Origin).HasColumnName("origin");
        });

        // --- Table: graffitis (entity Graffiti) ---
        modelBuilder.Entity<Graffiti>(e =>
        {
            e.ToTable("graffitis");
            e.Property(g => g.Id).HasColumnName("id");
            e.Property(g => g.RegisteredAt).HasColumnName("registered_at");
            e.Property(g => g.VisualDescription).HasColumnName("visual_description");
            e.Property(g => g.ThreatLevel).HasColumnName("threat_level");
            e.Property(g => g.GangId).HasColumnName("gang_id");
            e.Property(g => g.ImagePath).HasColumnName("image_path");

            // 1:N — DeleteBehavior.Restrict prevents deleting a gang that has graffitis
            e.HasOne(g => g.Gang)
             .WithMany(gang => gang.Graffitis)
             .HasForeignKey(g => g.GangId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Table: graffitis_location (entity GraffitiLocation) ---
        modelBuilder.Entity<GraffitiLocation>(e =>
        {
            e.ToTable("graffitis_location");
            e.Property(l => l.Id).HasColumnName("id");
            e.Property(l => l.Street).HasColumnName("street");
            e.Property(l => l.Neighborhood).HasColumnName("neighborhood");
            e.Property(l => l.City).HasColumnName("city");
            e.Property(l => l.State).HasColumnName("state");
            e.Property(l => l.Lat).HasColumnName("lat");
            e.Property(l => l.Lon).HasColumnName("lon");
            e.Property(l => l.GraffitiId).HasColumnName("graffiti_id");

            // 1:1 — GraffitiLocation is the dependent side
            e.HasOne(l => l.Graffiti)
             .WithOne(g => g.Location)
             .HasForeignKey<GraffitiLocation>(l => l.GraffitiId);

            // Unique index on FK enforces 1:1 at the database level
            e.HasIndex(l => l.GraffitiId).IsUnique();
        });
    }
}
