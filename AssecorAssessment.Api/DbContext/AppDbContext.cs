using AssecorAssessment.Api.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AssecorAssessment.Api.DbContext;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);

            entity.HasData(
                new Color { Id = 1, Name = "Blau" },
                new Color { Id = 2, Name = "Grün" },
                new Color { Id = 3, Name = "Violett" },
                new Color { Id = 4, Name = "Rot" },
                new Color { Id = 5, Name = "Gelb" },
                new Color { Id = 6, Name = "Türkis" },
                new Color { Id = 7, Name = "Weiß" }
            );
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Lastname).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Zipcode).IsRequired().HasMaxLength(10);
            entity.Property(p => p.City).IsRequired().HasMaxLength(100);

            entity.HasOne(p => p.Color)
                .WithMany()
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
