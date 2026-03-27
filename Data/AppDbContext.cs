using InventoryHub.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        modelBuilder.Entity<StockMovement>()
            .HasOne(sm => sm.Product)
            .WithMany(p => p.Movimentacoes)
            .HasForeignKey(sm => sm.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}