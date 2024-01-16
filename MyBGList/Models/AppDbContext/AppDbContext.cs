using Microsoft.EntityFrameworkCore;

namespace MyBGList.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BoardGame>()
            .HasOne(o => o.Publisher)
            .WithMany(m => m.BoardGames)
            .HasForeignKey(f => f.PublisherId)
            //.IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Categories>()
            .HasKey(k => new { k.BoardGameId, k.CategoryId });

        modelBuilder.Entity<BoardGames_Categories>()
            .HasOne(o => o.BoardGame)
            .WithMany(m => m.BoardGames_Categories)
            .HasForeignKey(f => f.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Categories>()
            .HasOne(o => o.Category)
            .WithMany(m => m.BoardGames_Categories)
            .HasForeignKey(f => f.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Domains>()
            .HasKey(k => new { k.BoardGameId, k.DomainId });

        modelBuilder.Entity<BoardGames_Domains>()
            .HasOne(o => o.BoardGame)
            .WithMany(m => m.BoardGames_Domains)
            .HasForeignKey(f => f.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Domains>()
            .HasOne(o => o.Domain)
            .WithMany(m => m.BoardGames_Domains)
            .HasForeignKey(f => f.DomainId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Mechanics>()
            .HasKey(k => new { k.BoardGameId, k.MechanicId });

        modelBuilder.Entity<BoardGames_Mechanics>()
            .HasOne(o => o.BoardGame)
            .WithMany(m => m.BoardGames_Mechanics)
            .HasForeignKey(f => f.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGames_Mechanics>()
            .HasOne(o => o.Mechanic)
            .WithMany(m => m.BoardGames_Mechanics)
            .HasForeignKey(f => f.MechanicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<BoardGame> BoardGames => Set<BoardGame>();

    public DbSet<BoardGames_Categories> BoardGames_Categories => Set<BoardGames_Categories>();

    public DbSet<BoardGames_Domains> BoardGames_Domains => Set<BoardGames_Domains>();

    public DbSet<BoardGames_Mechanics> BoardGames_Mechanics => Set<BoardGames_Mechanics>();

    public DbSet<Domain> Domains => Set<Domain>();

    public DbSet<Mechanic> Mechanics => Set<Mechanic>();

    public DbSet<Publisher> Publishers => Set<Publisher>();
}
